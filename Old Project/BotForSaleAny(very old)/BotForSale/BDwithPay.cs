using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiwi.BillPayments.Client;
using Qiwi.BillPayments.Model;
using Qiwi.BillPayments.Model.In;
using Microsoft.Win32;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Bson.Serialization;

namespace BotForSale
{
    class BDwithPay
    {
        private static string UserBillId { get; set; }
        readonly BillPaymentsClient clientQiwi = BillPaymentsClientFactory.Create(secretKey: "eyJ2ZXJzaW9uIjoiUDJQIiwiZGF0YSI6eyJwYXlpbl9tZXJjaGFudF9zaXRlX3VpZCI6ImVuZXQtMDAiLCJ1c2VyX2lkIjoiNzkyNjUwNjQyMzAiLCJzZWNyZXQiOiI0ZTBjMWJiNzg1MTBiZWM4MDY3OTM1MTRjMzdhYTQwNGJlNjBjMjRlNDgyZWRlNTJkYTFhMDVlYjE4ZWRmYmE2In19");


        public async Task<string> Method(int IdPeop, string FirstName, string LastName, string[] AccId)
        {
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("GamesAcc");
            var collection = database.GetCollection<PaymentsModel>("Payments");

            MainWork costAcc = new MainWork();
            decimal price = costAcc.CostsID(Program.id).GetAwaiter().GetResult();

            //Qiwi.BillPayments.Model.Out.BillResponse 
            var newPayment = clientQiwi.CreateBill(
                info: new CreateBillInfo
                {
                    BillId = Guid.NewGuid().ToString(),
                    Amount = new MoneyAmount
                    {
                        ValueDecimal = price,
                        CurrencyEnum = CurrencyEnum.Rub
                    },
                    Comment = "comment",
                    ExpirationDateTime = DateTime.Now.AddDays(45),
                    Customer = new Customer
                    {
                        Email = "example@mail.org",
                        Account = Guid.NewGuid().ToString(),
                        Phone = "79265064230"
                    }
                },
                customFields: new CustomFields
                {
                    ThemeCode = "кодСтиля"
                }
            );
            BDwithPay.UserBillId = newPayment.BillId.ToString();
            //var infopayment = clientQiwi.GetBillInfo(newPayment.BillId);
            var model = new PaymentsModel { Id = ObjectId.GenerateNewId(), FirstName = FirstName, LastName = LastName, IDPeop = IdPeop, Url = newPayment.PayUrl.ToString(), Status = newPayment.Status.ValueString.ToString(), BillId = newPayment.BillId.ToString(), BuyAcc= AccId};
            await collection.InsertOneAsync(model);
            string url = newPayment.PayUrl.ToString();
            return url;
        }
        public async Task<bool> UpdatePayments(string[] idAcc)
        {
            var client = new MongoClient("mongodb://localhost");
            var database = client.GetDatabase("GamesAcc");
            var paymentUpdate = database.GetCollection<PaymentsModel>("Payments");

            var localInfoUserBill = BDwithPay.UserBillId;
            int i = 0;
            string Status = "WAIT"; // можно любой текст
            while (Status != "PAID")
            {
                Status = clientQiwi.GetBillInfo(localInfoUserBill).Status.ValueString;
                i++;
                if(i == 600)
                {
                    paymentUpdate.UpdateOne(p => p.BillId == BDwithPay.UserBillId, Builders<PaymentsModel>.Update.Set(p => p.Status, "CANCEL"));
                    clientQiwi.CancelBill(billId: localInfoUserBill);
                    return false;
                }
                var taskdealy = Task.Delay(1000);
                await taskdealy;
            }
            
            paymentUpdate.UpdateOne(p => p.BillId == BDwithPay.UserBillId, Builders<PaymentsModel>.Update.Set(p => p.Status, Status));

            return true;
            

            
        }
    }
    [BsonIgnoreExtraElements]
    class PaymentsModel
    {
        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int IDPeop { get; set; }
        public string Url { get; set; }
        public string Status { get; set; }
        public string BillId { get; set; }
        public string[] BuyAcc { get; set; }
    }

}

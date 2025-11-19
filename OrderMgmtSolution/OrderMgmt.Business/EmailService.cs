using OrderMgmt.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace OrderMgmt.Business
{
    public class EmailService
    {
        private readonly SmtpSettings _settings;

        public EmailService(IOptions<SmtpSettings> options)
        {
            _settings = options.Value;
        }

        public void SendOrderConfirmation(Customer customer, Order order)
        {
            string confirmUrl = $"https://localhost:7281/Orders/ConfirmDelivery?token={order.DeliveryConfirmToken}";

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(_settings.User, "Order Management System");
                message.To.Add(customer.Email);
                message.Subject = $"Sipariş Onayı - {order.OrderNo}";
                message.Body =
                    $"Sayın {customer.CustomerName},\n\n" +
                    $"{order.OrderNo} numaralı siparişiniz oluşturuldu.\n" +
                    $"Ürün: {order.Product?.StockName}\n" +
                    $"Miktar: {order.Quantity}\n" +
                    $"Toplam Tutar: {order.TotalAmount}\n\n" +
                    $"Teslim aldığınızda onaylamak için:\n{confirmUrl}\n\n" +
                    $"Teşekkürler.";
                message.IsBodyHtml = false;

                using (var client = new SmtpClient(_settings.Server, _settings.Port))
                {
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_settings.User, _settings.Pass);

                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Mail başarıyla gönderildi.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Mail gönderilemedi: " + ex.ToString());
                    }
                }
            }
        }
    }
}

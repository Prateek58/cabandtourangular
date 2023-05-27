using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HillYatraAPI.ModelsCusom;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;

namespace HillYatraAPI
{
    public class EmailSend
    {
        public IConfiguration Configuration { get; set; }
        public EmailSend()
        {
        }

        public EmailSend(IConfiguration iconfig)
        {
            Configuration = iconfig;
        }
        public void SendMail(EmailModel data)
        {
            try
            {
                string username = Configuration.GetSection("Email").GetSection("username").Value;
                string password = Configuration.GetSection("Email").GetSection("password").Value;
                string host = Configuration.GetSection("Email").GetSection("host").Value;
                string port = Configuration.GetSection("Email").GetSection("port").Value;
                // create email message
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(username));
                email.To.Add(MailboxAddress.Parse(data.EmailTo));
                email.Cc.Add(MailboxAddress.Parse(data.EmailToCC));
                email.Subject = data.Subject;
            

                 var body = new TextPart(TextFormat.Html)
                 {
                     Text = data.Body//GetHtmlBody(req)
                 };

                email.Body = body;
               // MemoryStream stream = new MemoryStream(req.Pdf);
                //var attachment = new MimePart("application/pdf")
                //{
                //    Content = new MimeContent(stream),
                //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                //    ContentTransferEncoding = ContentEncoding.Base64,
                //    FileName = Path.GetFileName("enquiry-" + req.Name + "-" + req.Country + "-" + DateTime.Now.ToString())
                //};

                var multipart = new Multipart("mixed");
                multipart.Add(body);
                //multipart.Add(attachment);

                email.Body = multipart;

                using var smtp = new SmtpClient();
                smtp.Connect(host, Convert.ToInt32(port), SecureSocketOptions.Auto);
                smtp.Authenticate(username, password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {


            }


        }
        public string GetHtmlBody(EmailModel req)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("<h2>Enquery from {0}</h2>", req.EmailToFirstName));
            sb.Append(string.Format("<h2>Information-</h2>"));
            sb.Append(string.Format("<p>Name {0} {1}</p>",req.EmailToFirstName, req.EmailToLastName));
            //sb.Append(string.Format("<p>Contact {0}</p>", req.Contact));
            //sb.Append(string.Format("<p>Email {0}</p>", req.Email));
            //sb.Append(string.Format("<p>Country {0}</p>", req.Country));
            sb.Append(string.Format("<h2>Please find the attachment below-</h2>"));
            return sb.ToString();

        }
    }
}

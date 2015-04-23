using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using Messages.DeveloperTesting;

namespace TextMessageListener
{
    class Program
    {

        private static ConUContext db = new ConUContext();
        static void Main(string[] args)
        {
            using (var bus = RabbitHutch.CreateBus(ConfigurationManager.AppSettings["RabbitMQConnection"]))
            {
                bus.Subscribe<TextMessage>("CS-ContosoU", HandleTextMessage);

                Console.WriteLine("Listening for messages. Hit <return> to quit.");
                Console.ReadLine();
            }
        }

        private static void HandleTextMessage(TextMessage textMessage)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Got Message: {0}", textMessage.Text);

            var s = new Student()
            {
                FirstMidName = textMessage.Text,
                LastName = "ZConsole",
                EnrollmentDate = DateTime.Now
            };

            db.Students.Add(s);
            db.SaveChanges();

            Console.WriteLine("Student Added!");

            Console.ResetColor();
        }
    }
}

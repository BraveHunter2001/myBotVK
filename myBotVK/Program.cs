using System;
using System.Collections.Generic;
using System.IO;

using VkNet;
using VkNet.Categories;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VktestAPP
{
    class Program
    {
        static void Main(string[] args)
        {
            // //Вход как человек
            //var Me = new VkApi();

            // Me.Authorize(new ApiAuthParams
            // {
            //     ApplicationId = ,
            //     Login = "",
            //     Password = "",
            //     Settings = Settings.All
            // });
            // Console.WriteLine($"Пользователь подключен c id:{Me.UserId.Value}");

            // Вход бота
            var bot = new VkApi();

            long idbot = 197334937;
            var dictSubs = new Dictionary<long, string>();


            try
            {
                bot.Authorize(new ApiAuthParams
                {
                    AccessToken = ""
                });

                Console.WriteLine($"Bot connected with id:{idbot}");

                var listSubs = bot.Groups.GetMembers(new GroupsGetMembersParams { GroupId = idbot.ToString() });
                var listIdSubs = new List<long>();

                foreach (var sub in listSubs)
                {
                    listIdSubs.Add(sub.Id);
                }
                //listIdSubs.Remove(Me.UserId.Value);

                var listSubsU = bot.Users.Get(listIdSubs);

                // Словарь подписчиков ключ - id, значение - Имя Фамилия

                foreach (var sub in listSubsU)
                {
                    dictSubs.Add(sub.Id, sub.FirstName + " " + sub.LastName);
                }
                //dictSubs.Remove(Me.UserId.Value);

                //Вывод в консоль 
                Console.WriteLine("Subscribers of the group:");
                foreach (var sub in dictSubs)
                {
                    Console.WriteLine($"id:{sub.Key}| {sub.Value}");
                }
                Console.WriteLine("<-------------------------->");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error] {e.Message}");
            }



            // Запись подписчиков с id в файл subs.txt
            try
            {
                using (var sw = new StreamWriter(@"log\subs.txt"))
                {
                    foreach (var sub in dictSubs)
                    {
                        sw.WriteLine($"id:{sub.Key}| {sub.Value}");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Error] {e.Message}");
            }

            //Отправка сообщений
            Console.WriteLine("If you would quit writing messages, then input '/exit'");
            string message = "";
            do
            {
                Console.Write("Input text of message:");
                message = Console.ReadLine();
                if (message != "/exit")
                {
                    try
                    {
                        Random rndId = new Random();
                        bot.Messages.SendToUserIds(new MessagesSendParams
                        {
                            UserIds = dictSubs.Keys,
                            RandomId = rndId.Next(),
                            Message = $"[{DateTime.Now}] {message}"
                        });
                        Console.WriteLine("Message send");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"[Error] {e.Message}");
                    }
                }
            } while (message != "/exit");
            Console.WriteLine("Press enter to exit program...");

            Console.ReadLine();
        }
    }
}

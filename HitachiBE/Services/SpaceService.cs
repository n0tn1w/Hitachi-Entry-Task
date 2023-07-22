using EASendMail;
using HitachiBE.DB;
using HitachiBE.Migrations;
using HitachiBE.Models.Database;
using HitachiBE.Models.Request;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Runtime.Intrinsics.X86;

namespace HitachiBE.Services
{
    public class SpaceService : ISpaceService
    {
        private readonly ConectionDbContext _conectionDbContext;

        public SpaceService(ConectionDbContext conectionDbContext)
        {
            _conectionDbContext = conectionDbContext;
        }

        public async Task<string> CreateFile(IFormFile file)
        {
            //await _conectionDbContext.DaysDB.ExecuteDeleteAsync();

            var reader = new StreamReader(file.OpenReadStream());
            int numberOfElements = reader.ReadLine().ToString().Split(",").Length;

            List<DayDataModel> days = new List<DayDataModel>();
            for (int i = 0; i < numberOfElements-1; i++)
            {
                DayDataModel obj = new DayDataModel();
                obj.Id = Guid.NewGuid();
                obj.Score = 0;
                days.Add(obj);
            }
            
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".csv";
            using (StreamWriter writer = new StreamWriter(fileName))
            {

                while (reader.Peek() >= 0)
                {
                    string[] items = reader.ReadLine().ToString().Split(",");
                    string fieldName = items[0].Split(" ")[0].Split("/")[0];
                    PropertyInfo propertyInfo = typeof(DayDataModel).GetProperty(fieldName);


                    if (propertyInfo != null && propertyInfo.CanWrite)
                    {
                        string addFileToFile = "";

                        if (fieldName == "Lightning" || fieldName == "Clouds")
                        {
                            addFileToFile = fieldName + string.Join(",", new string[items.Length - 1]).TrimEnd();
                        }
                        else if (fieldName == "Id")
                        {
                            continue;
                        }
                        else
                        {
                            int max = -1;
                            int min = int.MaxValue;
                            int sum = 0;
                            List<int> medianArray = new List<int>();


                            for (int k = 1; k < numberOfElements; k++)
                            {
                                int data = Int32.Parse(items[k]);
                                min = data < min ? data : min;
                                max = data > max ? data : max;
                                sum += data;
                                medianArray.Add(data);
                            }


                            int len = items.Length;
                            int avg = (int)Math.Round((double)sum / len);

                            int median;
                            Array.Sort(medianArray.ToArray());
                            int mid = len / 2;
                            if (mid % 2 != 0)
                            {
                                median = medianArray[mid];
                            }
                            else
                            {
                                median = (int)Math.Round((double)((medianArray[mid] + medianArray[mid - 1]) / 2));
                            }


                            for (int k = 1; k < numberOfElements; k++)
                            {
                                propertyInfo.SetValue(days[k - 1], Int32.Parse(items[k]));
                            }

                            days.ForEach(day =>
                            {
                                if (day.Temperature < 2 || day.Temperature > 31 || day.Wind > 10 || day.Humidity >= 60
                                || day.Precipitation != 0 || day.Lightning || day.Clouds == "Cumulus" || day.Clouds == "Nimbus")
                                {
                                    day.Score = -1;
                                }
                                else
                                {
                                    if (fieldName == "Wind" || fieldName == "Humidity")
                                    {
                                        int score = (int)Math.Round((double)((int.Parse(propertyInfo.GetValue(day).ToString()) - min) * 100 / (max - min)));
                                        day.Score = day.Score == -1 ? -1 : day.Score + score;
                                    }
                                }

                            });

                            addFileToFile = fieldName + "," + avg + "," + min + "," + max + "," + median + ",";
                        }

                        writer.WriteLine(addFileToFile);
                    }
                }
            }

            DayDataModel? highestScoreObject = days.OrderByDescending(obj => obj.Score).FirstOrDefault();
            string[] lines = File.ReadAllLines(fileName);
            PropertyInfo[] properties = highestScoreObject != null ? highestScoreObject.GetType().GetProperties() : null;

            // Iterate over each line
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string appendedLine;

                if (highestScoreObject != null)
                {
                    PropertyInfo fieldName = properties[i - 1];

                    if (fieldName.Name == "Lightning" || fieldName.Name == "Clouds")
                    {
                        appendedLine = ",";
                    }
                    else if (fieldName.Name == "Id" || fieldName.Name == "Day")
                    {
                        continue;
                    } else 
                    {
                        appendedLine = "" + fieldName.GetValue(highestScoreObject);
                    }
                }
                else
                {
                    appendedLine = ",";
                }

                // Update the line in the array
                lines[i] = line + appendedLine;
            }

            // Write the updated lines back to the file
            File.WriteAllLines(fileName, lines);


            //await _conectionDbContext.DaysDB.AddRangeAsync(days);
            //await _conectionDbContext.SaveChangesAsync();

            return fileName;
        }


        public async Task SendEmail(EmailDataRequest input)
        {
            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");

                // Your yahoo email address
                oMail.From = input.sender;

                // Set recipient email address
                oMail.To = input.receiver;

                // Set email subject
                oMail.Subject = "test email from yahoo account";

                // Set email body
                oMail.TextBody = "this is a test email sent from c# with yahoo.";

                var fileName = await CreateFile(input.file);
                oMail.AddAttachment(fileName);

                // Yahoo SMTP server address
                SmtpServer oServer = new SmtpServer("smtp.abv.bg");

                // For example: your email is "myid@yahoo.com", then the user should be "myid@yahoo.com"
                oServer.User = input.sender;
                oServer.Password = input.password;


                // Because yahoo deploys SMTP server on 465 port with direct SSL connection.
                // So we should change the port to 465. you can also use 25 or 587
                oServer.Port = 465;


                // detect SSL type automatically
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

                EASendMail.SmtpClient oSmtp = new EASendMail.SmtpClient();


                oSmtp.SendMail(oServer, oMail);


            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
            }
        }

        //private async Task<string> CreateFile() {
        //    string fileName = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".csv";
        //    List<DayDataModel> list = await _conectionDbContext.DaysDB.ToListAsync();
        //    List<IndentifierDayDataModel> indetList = new List<IndentifierDayDataModel>();
        //    DayDataModel best = CalculateBestDay(list);

        //    using (StreamWriter writer = new StreamWriter(fileName))
        //    {
        //        PropertyInfo[] properties = typeof(DayDataModel).GetProperties();

        //        for (int i = 0; i < properties.Length; i++)
        //        {

        //            if (properties[i] != null)
        //            {
        //                if (properties[i].Name == "Lightning")
        //                {
        //                    //propertyInfo.SetValue(days[i - 1], items[i] == "Yes" ? true : false);
        //                }
        //                else if (properties[i].Name == "Clouds")
        //                {
        //                    //propertyInfo.SetValue(days[i - 1], items[i]);
        //                }
        //                else
        //                {
        //                    string dataToWriteIntoFile = ByPropertyNameReturnOutputString(properties[i], list);
        //                    writer.Write(dataToWriteIntoFile);
        //                    var data = properties[i].GetValue(best);
        //                    writer.WriteLine(data.ToString());
        //                }
        //            }
        //            else
        //            {
        //                throw new InvalidOperationException();
        //            }
        //        }
        //    }

        //    return fileName;
        //}


        //private string ByPropertyNameReturnOutputString(PropertyInfo property, List<DayDataModel> list) {
        //    int min = -1;
        //    int max = int.MaxValue;
        //    int sum = 0;
        //    List<int> medianArray = new List<int>();

        //    list.ForEach(day =>
        //    {
        //        int data = Int32.Parse((string)property.GetValue(day));
        //        min = data < min ? data : min;
        //        max = data > max ? data : max;
        //        sum += data;
        //        medianArray.Add(data);
        //    });

        //    int len = list.Count;
        //    int avg = (int)Math.Round((double)sum / len);

        //    int median;
        //    Array.Sort(medianArray.ToArray());
        //    int mid = len / 2;
        //    if (mid % 2 != 0)
        //    {
        //        median = medianArray[mid];
        //    }
        //    else 
        //    {
        //        median = (int)Math.Round((double)((medianArray[mid] + medianArray[mid-1]) / 2)); 
        //    }


        //    return avg + "," + min + "," + max + "," + median;
        //}
    }
}

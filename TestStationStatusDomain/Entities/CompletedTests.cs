using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.IO;

namespace TestStationStatusDomain.Entities
{
    public class CompletedTest
    {
        [Key]
        public virtual int Id { get; set; }
        public virtual string TestStationName { get; set; }
        public virtual string TestCaseName { get; set; }
        public virtual string LogFileName { get; set; }
        public List<string> Commands = new List<string>();
        public List<string> Results = new List<string>();
        public virtual double DurationSeconds { get; set; }

        public string CommandsMeta
        {
            get
            {

                var serializer = new DataContractJsonSerializer(typeof(List<string>));
                var stream = new MemoryStream();
                serializer.WriteObject(stream, Commands);
                stream.Position = 0;
                var jsonBody = new StreamReader(stream).ReadToEnd();
                return jsonBody;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    Commands.Clear();
                else
                {
                    var outputSerializer = new DataContractJsonSerializer(typeof(List<string>));
                    MemoryStream destream = new MemoryStream(Encoding.UTF8.GetBytes(value));
                    if (destream.Length > 0)
                    {
                        Commands = (List<string>)outputSerializer.ReadObject(destream);
                    }
                }
            }

        }

        public string ResultsMeta
        {
            get
            {

                var serializer = new DataContractJsonSerializer(typeof(List<string>));
                var stream = new MemoryStream();
                serializer.WriteObject(stream, Results);
                stream.Position = 0;
                var jsonBody = new StreamReader(stream).ReadToEnd();
                return jsonBody;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    Results.Clear();
                else
                {
                    var outputSerializer = new DataContractJsonSerializer(typeof(List<string>));
                    MemoryStream destream = new MemoryStream(Encoding.UTF8.GetBytes(value));
                    if (destream.Length > 0)
                    {
                        Results = (List<string>)outputSerializer.ReadObject(destream);
                    }
                }
            }

        }



    }
}

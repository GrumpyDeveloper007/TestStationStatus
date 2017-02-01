using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace TestStationStatusDomain.Entities
{
    public class StatusUpdate
    {
        [Key]
        public virtual string TestStationName { get; set; }
        public virtual string Status { get; set; }
        public virtual string TestCaseName { get; set; }
        public virtual string LogFileName { get; set; }
        public virtual double DurationSeconds { get; set; }
        public List<string> Last10Commands = new List<string>();
        public List<string> Last10Results = new List<string>();

        public string CommandsMeta
        {
            get
            {

                var serializer = new DataContractJsonSerializer(typeof(List<string>));
                var stream = new MemoryStream();
                serializer.WriteObject(stream, Last10Commands);
                stream.Position = 0;
                var jsonBody = new StreamReader(stream).ReadToEnd();
                return jsonBody;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    Last10Commands.Clear();
                else
                {
                    var outputSerializer = new DataContractJsonSerializer(typeof(List<string>));
                    MemoryStream destream = new MemoryStream(Encoding.UTF8.GetBytes(value));
                    if (destream.Length > 0)
                    {
                        Last10Commands = (List<string>)outputSerializer.ReadObject(destream);
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
                serializer.WriteObject(stream, Last10Results);
                stream.Position = 0;
                var jsonBody = new StreamReader(stream).ReadToEnd();
                return jsonBody;
            }

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    Last10Results.Clear();
                else
                {
                    var outputSerializer = new DataContractJsonSerializer(typeof(List<string>));
                    MemoryStream destream = new MemoryStream(Encoding.UTF8.GetBytes(value));
                    if (destream.Length > 0)
                    {
                        Last10Results = (List<string>)outputSerializer.ReadObject(destream);
                    }
                }
            }

        }


    }
}

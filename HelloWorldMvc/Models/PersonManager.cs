﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;

namespace HelloWorldMvc.Models
{
    public class PersonManager
    {
        
        public static string DefaultFileBin = "App_Data/PersonManager.bin";

        public static string DefaultFileXml = "App_Data/PersonManager.xml";
        
        public static List<string> Personlist { get; protected set; } = new List<string>()
        {
            "pierre",
            "paul",
            "jacques",
        };

        public static List<Person> DefaultPersonList { get; protected set; } = new List<Person>()
        {
            new Person(1, "Didier", "Responsable de formation"),
            new Person(2, "Sophie", "Formatrice"),
            new Person(3, "Franck", "Formateur"),
            new Person(4, "Mickaël", "Formateur"),
        };


        string fileName;

        string binPath;

        string xmlPath;

        bool binExists;

        bool xmlExists;

        public List<Person> personList;

        public PersonManager() : this(DefaultFileBin)
        {
            
        }

        public PersonManager(string _file)
        {
            if(HttpContext.Current != null)
            {
                fileName = HttpContext.Current.Server.MapPath(DefaultFileXml);
            }
            else
            {
                fileName = _file;
            }

            binPath = fileName + ".bin";
            xmlPath = fileName + ".xml";

            binExists = File.Exists(binPath);
            xmlExists = File.Exists(xmlPath);

            personList = DefaultPersonList;
        }
        
        public Person Search(int id)
        {
            Person member = DefaultPersonList.FirstOrDefault(item => item.Id == id);

            return (member != default(Person)) ? member : new Person();
        }

        public static Person Search(string name)
        {
            Person member = DefaultPersonList.FirstOrDefault(item => item.Name.ToLower() == name.ToLower());

            return (member != default(Person)) ? member : new Person();
        }

        public static Person Search(Person p)
        {
            return Search(p.Id);
        }

        public static List<Person> SearchJob(string job)
        {
            return DefaultPersonList.FindAll(item => item.Job.ToLower() == job.ToLower());
        }


        public static void Save()
        {
            
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(HttpContext.Current.Server.MapPath(DefaultFileBin), FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, DefaultPersonList);
            stream.Close();
            SaveXml();
        }

        public static void Load()
        {
            try
            {
                if(File.Exists(HttpContext.Current.Server.MapPath(DefaultFileBin)))
                {
                    IFormatter formatter = new BinaryFormatter();
                    Stream stream = new FileStream(HttpContext.Current.Server.MapPath(DefaultFileBin), FileMode.Open, FileAccess.Read, FileShare.Read);
                    List<Person> loaded = formatter.Deserialize(stream) as List<Person>;
                    stream.Close();
                    if(loaded != null)
                    {
                        DefaultPersonList = loaded;
                    }
                }
            }
            catch
            {

            }
        }

        public static void SaveXml()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Person>));

            using (StreamWriter writer = new StreamWriter(HttpContext.Current.Server.MapPath(DefaultFileXml)))
            {
                serializer.Serialize(writer, DefaultPersonList);
            }
        }

        public static void LoadXml()
        {
            try
            {
                if(File.Exists(HttpContext.Current.Server.MapPath(DefaultFileXml)))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Person>));

                    using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(DefaultFileXml)))
                    {
                        List<Person> loaded = serializer.Deserialize(reader) as List<Person>;

                        if (loaded != null)
                        {
                            DefaultPersonList = loaded;
                        }
                    }
                }
                
            }
            catch
            {

            }
        }

    }
}
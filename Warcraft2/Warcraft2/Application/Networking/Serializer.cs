using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using System.Reflection;

namespace Warcraft2.Application.Networking
{
    public static class Serializer
    {

        private static Type[] types;
        public static Dictionary<Type, XmlSerializer> dictionary = new Dictionary<Type, XmlSerializer>();

        public static Object DeserializeObject<Object>(this string toDeserialize)
        {
            XmlSerializer xmlSerializer = GetSerializer(typeof(Object));
            StringReader textReader = new StringReader(toDeserialize);

            return (Object)xmlSerializer.Deserialize(textReader);
        }

        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer serializerObject = GetSerializer(typeof(T));

            using(StringWriter writer = new StringWriter())
            {
                serializerObject.Serialize(writer, toSerialize);

                return writer.ToString();
            }
        }

        private static Type[] GetExtraTypes()
        {
            if(types == null)
            {
                List<Type> tempTypes = new List<Type>();
                List<String> classes = new List<String>();
                classes.AddRange(GetClasses("StrategyGame.App.Core.Gameplay.Players"));
                //classes.AddRange(GetClasses("StrategyGame.App.Core.Gameplay.Players.Actions"));
                //classes.AddRange(GetClasses("StrategyGame.App.Core.Gameplay"));
                //classes.AddRange(GetClasses("StrategyGame.App.Core.Gameplay.Tiles"));
                //classes.AddRange(GetClasses("StrategyGame.App.Core.Gameplay.Tiles.Buildings"));
                //classes.AddRange(GetClasses("StrategyGame.App.Core.Gameplay.Tiles.ResourceTiles"));

                foreach(String str in classes)
                {
                    Type t = Type.GetType(str);
                    if(!tempTypes.Contains(t))
                    {
                        tempTypes.Add(t);
                    }
                }

                types = new Type[tempTypes.Count];
                int counter = 0;
                foreach(Type t in tempTypes)
                {
                    types[counter] = t;
                    counter++;
                }
            }

            return types;
        }

        private static List<string> GetClasses(string nameSpace)
        {
            Assembly asm = Assembly.GetExecutingAssembly();

            List<string> namespacelist = new List<string>();
            List<string> classlist = new List<string>();

            foreach(Type type in asm.GetTypes())
            {
                if(type.Namespace == nameSpace && !type.IsAbstract)
                {
                    namespacelist.Add(type.AssemblyQualifiedName);
                }
            }

            foreach(string classname in namespacelist)
                classlist.Add(classname);

            return classlist;
        }

        private static XmlSerializer GetSerializer(Type T)
        {
            if(!dictionary.ContainsKey(T))
            {
                XmlSerializer cereal = new XmlSerializer(T, GetExtraTypes());
                dictionary.Add(T, cereal);
            }

            return dictionary[T];
        }
    }
}

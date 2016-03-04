using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace SharkData.Models
{
    public class Shark : NotifyBase
    {
        private int _id;
        private string _name;
        private string _binomial;
        private int _maxLength;

        public int Id { get { return _id; } set { SetProperty(ref _id, value); } }
        public string Name { get { return _name; } set { SetProperty(ref _name, value); } }
        public string Binomial { get { return _binomial; } set { SetProperty(ref _binomial, value); } }
        public int MaxLength { get { return _maxLength; } set { SetProperty(ref _maxLength, value); } }

        public JsonObject ToJson()
        {
            JsonObject jsonObj = new JsonObject();
            jsonObj.SetNamedValue(nameof(Id), JsonValue.CreateNumberValue(Id));
            jsonObj.SetNamedValue(nameof(Name), JsonValue.CreateStringValue(Name));
            jsonObj.SetNamedValue(nameof(Binomial), JsonValue.CreateStringValue(Binomial));
            jsonObj.SetNamedValue(nameof(MaxLength), JsonValue.CreateNumberValue(MaxLength));
            return jsonObj;
        }

        public static Shark ConvertFromJson(string json)
        {
            JsonObject jsonObj = JsonObject.Parse(json);
            return new Shark()
            {
                Id = (int)jsonObj.GetNamedNumber("Id"),
                Name = jsonObj.GetNamedString("Name"),
                Binomial = jsonObj.GetNamedString("Binomial"),
                MaxLength = (int)jsonObj.GetNamedNumber("MaxLength")
            };
        }
    }
}

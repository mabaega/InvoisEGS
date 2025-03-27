using System.Globalization;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml;

namespace InvoisEGS.ApiClient.XModels
{
    public class DecimalXmlConverter : IXmlSerializable
    {
        private decimal _value;

        public DecimalXmlConverter() { }

        public DecimalXmlConverter(decimal value)
        {
            _value = value;
        }

        public static implicit operator decimal(DecimalXmlConverter d) => d._value;
        public static implicit operator DecimalXmlConverter(decimal d) => new DecimalXmlConverter(d);

        public XmlSchema GetSchema() => null;

        public void ReadXml(XmlReader reader)
        {
            if (decimal.TryParse(reader.ReadElementContentAsString(), NumberStyles.Float, CultureInfo.InvariantCulture, out decimal result))
            {
                _value = result;
            }
            else
            {
                _value = 0; // Default jika parsing gagal
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(_value.ToString("0.000#######", CultureInfo.InvariantCulture));
        }
    }
}

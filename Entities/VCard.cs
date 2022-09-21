using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class VCard : IEntity
    {
      
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public override string ToString()
        {
            return base.ToString();

            var builder = new StringBuilder();
            builder.AppendLine("BEGIN:VCARD");

            // Full name        
            builder.Append("FN:").Append(Firstname)
              .Append(" ").AppendLine(Surname);

            // Name        
            builder.Append("N:d'").Append(Surname)
              .Append(";").AppendLine(Firstname);

            // Address        
            builder.Append("ADR:").Append(City).Append(";")
              .AppendLine(Country);

            // Contact infos      
            builder.Append("SORT-STRING:").AppendLine(Surname);
            builder.Append("TEL:").AppendLine(Phone);
            builder.Append("EMAIL;PREF;INTERNET:").AppendLine(Email);

            builder.AppendLine("END:VCARD");
            return builder.ToString();
        }
    }
}

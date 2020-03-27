using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.DBModels
{
    public class ApplicationUser : IdentityUser
    { 
        [MaxLength(50)]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [Display(Name = "Created")]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Updated")]
        [DataType(DataType.Date)]
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public DateTime? Blocked { get; set; }
        public string BlockedBy { get; set; }

       
        public ICollection<ApplicationUserRole> UserRoles { get; set; }  //https://entityframeworkcore.com/knowledge-base/51004516/-net-core-2-1-identity-get-all-users-with-their-associated-roles

        [JsonIgnore]
        public override string UserName
        {
            get { return base.UserName; }
            set { base.UserName = value; }
        }

        [JsonIgnore]
        public override string NormalizedUserName
        {
            get { return base.NormalizedUserName; }
            set { base.NormalizedUserName = value; }
        }
        [JsonIgnore]
        public override string Email
        {
            get { return base.Email; }
            set { base.Email = value; }
        }
        [JsonIgnore]
        public override string NormalizedEmail
        {
            get { return base.NormalizedEmail; }
            set { base.NormalizedEmail = value; }
        }

        [JsonIgnore]
        public override string PasswordHash
        {
            get { return base.PasswordHash; }
            set { base.PasswordHash = value; }
        }
        [JsonIgnore]
        public override string SecurityStamp
        {
            get { return base.SecurityStamp; }
            set { base.SecurityStamp = value; }
        }
        [JsonIgnore]
        public override string ConcurrencyStamp
        {
            get { return base.ConcurrencyStamp; }
            set { base.ConcurrencyStamp = value; }
        }
        [JsonIgnore]
        public override string PhoneNumber
        {
            get { return base.PhoneNumber; }
            set { base.PhoneNumber = value; }
        }
        [JsonIgnore]
        public override bool PhoneNumberConfirmed
        {
            get { return base.PhoneNumberConfirmed; }
            set { base.PhoneNumberConfirmed = value; }
        }
        [JsonIgnore]
        public override bool TwoFactorEnabled
        {
            get { return base.TwoFactorEnabled; }
            set { base.TwoFactorEnabled = value; }
        }
        [JsonIgnore]
        public override bool EmailConfirmed
        {
            get { return base.EmailConfirmed; }
            set { base.EmailConfirmed = value; }
        }
        [JsonIgnore]
        public override DateTimeOffset? LockoutEnd
        {
            get { return base.LockoutEnd; }
            set { base.LockoutEnd = value; }
        }
        [JsonIgnore]
        public override bool LockoutEnabled
        {
            get { return base.LockoutEnabled; }
            set { base.LockoutEnabled = value; }
        }

        [JsonIgnore]
        public override int AccessFailedCount
        {
            get { return base.AccessFailedCount; }
            set { base.AccessFailedCount = value; }
        }
    }
}

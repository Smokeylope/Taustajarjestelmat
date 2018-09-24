using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Layered
{
    public enum ItemType
    {
        Sword,
        Bow,
        Shield,
        Armor
    };

    public class Item
    {
        public Guid Id { get; set; }
        [Range(0,99)]
        public int Level { get; set; }
        [Range(0,3)]
        public ItemType Type { get; set; }
        [TimeInPast]
        public DateTime CreationTime { get; set; }
    }

    public class NewItem
    {
        [Range(0,99)]
        public int Level {get; set; }
        [Range(0,3)]
        public ItemType Type { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class ModifiedItem
    {
        [Range(0,3)]
        public ItemType Type { get; set; }
    }

    public class TimeInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Item item = (Item) validationContext.ObjectInstance;

            if (item.CreationTime > DateTime.Now)
            {
                return new ValidationResult("Creation time must be in the past");
            }

            return ValidationResult.Success;
        }
    }
}
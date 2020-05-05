using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NovaPochta.API.Domain
{
    public class Cargo
    {
        public CargoIdentifier CargoIdentifier { get; set; }
        public float Weight { get; set; }
        public IEnumerable<CargoItem> Items { get; set; }
    }

    public class CargoItem
    {
        public Size Size { get; set; }
        public Decsription Description { get; set; }
    }

    public class Decsription : IValidatableObject
    {
        public string Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var description = validationContext.ObjectInstance as Decsription;
            if (description != null)
            {
                if (!string.IsNullOrWhiteSpace(description.Value) && description.Value.Length < byte.MaxValue)
                {
                    yield return ValidationResult.Success;
                }
            }

            yield return new ValidationResult($"{nameof(Decsription)} is invalid.");
        }
    }

    public class Size : ValueObject
    {
        private float _width;
        private float _height;
        private float _length;

        public float Width
        {
            get => _width;
            set => _width = ValidateSize(value);
        }

        public float Height
        {
            get => _height;
            set => _height = ValidateSize(value);
        }

        public float Length
        {
            get => _length;
            set => _length = ValidateSize(value);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Width;
            yield return Height;
            yield return Length;
        }

        private float ValidateSize(float size)
        {
            if (size < default(float))
            {
                throw new ValidationException($"{size} is invalid.");
            }

            return size;
        }
    }

    public class CargoIdentifier : IValidatableObject
    {
        public string Value { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (validationContext.ObjectInstance is CargoIdentifier identifier)
            {
                if (!string.IsNullOrWhiteSpace(identifier.Value) && identifier.Value.Trim().Length == 16)
                {
                    yield return ValidationResult.Success;
                }
            }

            yield return new ValidationResult("Identifier is invalid.");
        }
    }
}

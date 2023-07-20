using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optionals
{
    public class Optional<Ty> 
    {
        private Ty? _value = default(Ty);
        public Ty Value {
            get {
                if (HadError || _value == null)
                    throw new InvalidOperationException("unable to retrieve optional value. Optional value is storing an error");
                return _value;
            } 
        }
        public bool HadError { get; private set; }
        public string? ErrorMessage { get; set; }

        public Optional() { }
        public Optional(Ty value)
        {
            _value = value;
        }
        public Optional(string err, bool _ = false)
        {
            ErrorMessage = err;
            HadError = true;
        }
        public static Optional<Ty> FromError(string msg)
        {
            return new Optional<Ty>(msg);
        }

        public static Optional<Ty> FromValue(Ty value)
        {
            return new Optional<Ty>(value);
        }
    }

    public static class Optional
    {
        public static Optional<TyVal> FromError<TyVal>(string msg)
        {
            return new Optional<TyVal>(msg);
        }

        public static Optional<TyVal> FromValue<TyVal>(TyVal value)
        {
            return new Optional<TyVal>(value);
        }
    }
}

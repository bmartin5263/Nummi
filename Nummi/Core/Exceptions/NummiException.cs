namespace Nummi.Core.Exceptions; 

public abstract class NummiException : Exception {
    protected NummiException(string message) : base(message) {
    }

    protected NummiException(string message, Exception causedBy) : base(message, causedBy) {
    }
}
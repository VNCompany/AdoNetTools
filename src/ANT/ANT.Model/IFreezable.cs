namespace ANT.Model
{
    public interface IFreezable
    {
        bool IsFrozen { get; }
        void Freeze();
    }
}
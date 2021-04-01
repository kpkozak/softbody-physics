namespace Physics.Constraints
{
    public interface IConstraint
    {
        void Prepare();
        void Resolve();
    }
}
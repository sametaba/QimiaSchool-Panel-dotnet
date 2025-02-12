namespace QimiaSchool.DataAccess.Exceptions
{
    public class EntityNotFoundException<T> : Exception
    {
        public EntityNotFoundException(object key)
            : base($"Entity '{typeof(T).Name}' ({key}) was not found.")
        {
        }
    }
}

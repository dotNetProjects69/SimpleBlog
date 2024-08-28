namespace SimpleBlog.MVC.Shared
{
    public interface ISessionHandler
    {
        public int GetOwnerId();
        public void SetOwnerId(int id);

        public void SetUnauthorizedId();
    }
}

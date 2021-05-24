namespace Belphegor
{
    public class IdleStateToggler : IToggleIdle
    {
        private bool _isIdleVerifyEnabled = false;

        public bool IsIdleVerifyEnabled() => _isIdleVerifyEnabled;

        public void ToggleIdleVerify() => _isIdleVerifyEnabled = !_isIdleVerifyEnabled;
    }
}
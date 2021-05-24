namespace Belphegor
{
    public class ExecutionStateSwitchingIdleStateTogglingDecorator : IToggleIdle
    {
        private readonly IToggleIdle _inner;

        public ExecutionStateSwitchingIdleStateTogglingDecorator(IToggleIdle inner)
        {
            _inner = inner;
        }

        public bool IsIdleVerifyEnabled() => _inner.IsIdleVerifyEnabled();

        public void ToggleIdleVerify()
        {
            _inner.ToggleIdleVerify();
            if (_inner.IsIdleVerifyEnabled())
            {
                ExecutionStateChanger.PreventIdle();
            }
            else
            {
                ExecutionStateChanger.AllowIdle();
            }
        }
    }
}
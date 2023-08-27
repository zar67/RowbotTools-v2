using RowbotTools.UI.ViewSystem;

public class TestState : State
{
    public override void Enter()
    {
        base.Enter();

        m_viewService.Open<TestView>();
    }
}

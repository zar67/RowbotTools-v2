using RowbotTools.UI.ViewSystem;

public class TestState : State
{
    protected override void OpenViews()
    {
        base.OpenViews();

        m_viewService.Open<TestView>();
    }
}

namespace Engine.UI
{
    public class UIWidget: UIElement
    {
        public override void Close()
        {
            Destroy(true);
        }
    }
}
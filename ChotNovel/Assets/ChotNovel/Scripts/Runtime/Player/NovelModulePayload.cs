namespace ChotNovel.Player
{
    public class NovelModulePayload
    {
        // Control by player.
        public NovelPlayer Player;
        public bool IgnoreWait;

        // Control by module.
        public bool SkipToEndOfPage;
    }
}

namespace ChotNovel.Player
{
    public class NovelModulePayload
    {
        // Control by player.
        public NovelPlayer Player;
        public bool IgnoreWait;
        public bool IgnoreJump;

        // Control by module.
        public bool SkipToEndOfPage;
    }
}

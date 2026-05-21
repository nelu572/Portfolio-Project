namespace PortfolioFilling.Weapon
{
    public sealed class AmmoSystem
    {
        public int CurrentClip { get; private set; }
        public int CurrentReserve { get; private set; }
        public int ClipSize { get; }

        public AmmoSystem(int clipSize, int reserveAmmo)
        {
            ClipSize = clipSize;
            CurrentClip = clipSize;
            CurrentReserve = reserveAmmo;
        }

        public bool CanFire() => CurrentClip > 0;
        public bool CanReload() => CurrentClip < ClipSize && CurrentReserve > 0;

        public void ConsumeRound()
        {
            if (CurrentClip > 0)
            {
                CurrentClip--;
            }
        }

        public void ReloadFull()
        {
            var needed = ClipSize - CurrentClip;
            var moved = needed > CurrentReserve ? CurrentReserve : needed;
            CurrentClip += moved;
            CurrentReserve -= moved;
        }

        public void Refill()
        {
            CurrentClip = ClipSize;
            CurrentReserve = ClipSize * 6;
        }
    }
}

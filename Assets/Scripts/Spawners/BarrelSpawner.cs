public class BarrelSpawner : Spawner<Barrel>
{
    protected override void ActionOnGet(Barrel barrel)
    {
        barrel.gameObject.SetActive(true);
        barrel.OnCollected += ReleaseObject;
    }

    protected override void ActionOnRelease(Barrel barrel)
    {
        barrel.gameObject.SetActive(false);
        barrel.OnCollected -= ReleaseObject;
    }

    protected override void ReleaseObject(Barrel barrel)
    {
        Pool.Release(barrel);
        barrel.ResetCondition();
    }
}

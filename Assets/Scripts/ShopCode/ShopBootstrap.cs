using UnityEngine;
using Runner.Core;

public class ShopBootstrap : MonoBehaviour
{
    [SerializeField] private Shop _shop;
    [SerializeField] private WalletView _walletView;

    private IDataProvider _dataProvider;
    private IPersistentData _persistentPlayerData;

    private Wallet _wallet;

    public void Awake()
    {
        TryUseSharedRuntimeData();
        
        if (_persistentPlayerData == null || _dataProvider == null)
            InitializeData();
        
        if (_wallet == null)
            InitializeWallet();
        else if (_walletView != null)
            _walletView.Initialize(_wallet);
        
        InitializeShop();
    }

    private void TryUseSharedRuntimeData()
    {
        _persistentPlayerData = RunnerRuntimeContext.PersistentData;
        _dataProvider = RunnerRuntimeContext.DataProvider;
        _wallet = RunnerRuntimeContext.Wallet;
    }

    private void InitializeData()
    {
        _persistentPlayerData = new PersistentData();
        _dataProvider = new DataLocalProvider(_persistentPlayerData);

        LoadDataOrInit();
    }

    private void InitializeWallet()
    {
        _wallet = new Wallet(_persistentPlayerData);

        if (_walletView != null)
            _walletView.Initialize(_wallet);
    }

    private void InitializeShop()
    {
        OpenSkinsChecker openSkinsChecker = new OpenSkinsChecker(_persistentPlayerData);
        SelectedSkinChecker selectedSkinChecker = new SelectedSkinChecker(_persistentPlayerData);
        SkinSelector skinSelector = new SkinSelector(_persistentPlayerData);
        SkinUnlocker skinUnlocker = new SkinUnlocker(_persistentPlayerData);

        _shop.Initialize(_dataProvider, _wallet, openSkinsChecker, selectedSkinChecker, skinSelector, skinUnlocker);
    }

    private void LoadDataOrInit()
    {
        if (_dataProvider.TryLoad() == false)
            _persistentPlayerData.PlayerData = new PlayerData();
    }
}

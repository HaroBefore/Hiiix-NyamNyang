using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using UnityEngine.Purchasing.Security;
using UnityEngine.Analytics;
public class IAPManager : MonoBehaviour, IStoreListener {

    public static IAPManager instance;

    static IStoreController storeController;
    static string productID_removeAds;

    //private IExtensionProvider extension;

    void Awake() {
        if (!instance) instance = this;
        if (storeController == null) {
            productID_removeAds = "removeadvertise";
            InitStore();
        }
    }

    void InitStore() {
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(productID_removeAds, ProductType.NonConsumable, new IDs { { productID_removeAds, GooglePlay.Name } });
        


        UnityPurchasing.Initialize(this, builder);
    }

    void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        storeController = controller;
        //extension = extensions;
    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error) {
        Debug.Log("OnInitializeFailed" + error);
    }

    public void PurchaseItem_RemoveAds() {
        if (storeController == null)
            Debug.Log("구매 실패: 결제 기능 초기화 실패.");
        else
            storeController.InitiatePurchase(productID_removeAds);
    }

    PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs e) {
        bool isSuccess = true;

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        CrossPlatformValidator validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
try{
IPurchaseReceipt[] result = validator.Validate(e.purchasedProduct.receipt);
for(int i=0;i<result.Length;i++)
Analytics.Transaction(result[i].productID, e.purchasedProduct.metadata.localizedPrice, e.purchasedProduct.metadata.isoCurrencyCode, result[i].transactionID, null);
}
catch(IAPSecurityException){
isSuccess = false;
}
#endif
        if (isSuccess) {
            Debug.Log("구매 완료");
            if (e.purchasedProduct.definition.id.Equals(productID_removeAds))
                RemoveAdvertise();
        }
        else
            Debug.Log("구매 실패: 비정상 결제.");
        return PurchaseProcessingResult.Pending;
    }

    void IStoreListener.OnPurchaseFailed(Product i, PurchaseFailureReason error) {
        if (!error.Equals(PurchaseFailureReason.UserCancelled)) {
            Debug.Log("구매 실패: " + error);
        }
    }

    void RemoveAdvertise() {
        AdsManager_Admob.instance.RemoveAds();
    }

    public void RestoreTransactions() {
        //extension.GetExtension<IAppleExtensions>().RestoreTransactions(result => {
        //    if (result) {
        //        RemoveAdvertise();
        //    }
        //    else {
        //        // Restoration failed.
        //    }
        //});
    }
}
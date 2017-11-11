using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener {
	private static IStoreController m_StoreController;          // The Unity Purchasing system.
	private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

	const string PRODUCT_ID = "bailout_package";

	public static IAPManager Instance;

	void Awake (){
		if (Instance == null) {
			Instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		if (m_StoreController == null){
			InitializePurchasing();
		}
	}

	public void InitializePurchasing() {
		if (IsInitialized()){
			Debug.Log ("Already Initialized");
			return;
		}

		var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		builder.AddProduct(PRODUCT_ID, ProductType.Consumable);

		UnityPurchasing.Initialize(this, builder);
		Debug.Log ("Init attempted");
	}


	private bool IsInitialized() {
		// Only say we are initialized if both the Purchasing references are set.
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}


	public void BuyProduct() {
		if (IsInitialized()) {
			Product product = m_StoreController.products.WithID(PRODUCT_ID);

			if (product != null && product.availableToPurchase){
				Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
				m_StoreController.InitiatePurchase(product);

			} else {
				Debug.Log("BuyProduct: FAIL. Not purchasing product, either is not found or is not available for purchase");
				PurchaseFailed ();
			}
		} else {
			Debug.Log("BuyProduct FAIL. Not initialized.");
			PurchaseFailed ();
		}
	}


	//  
	// --- IStoreListener
	//

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions){
		Debug.Log("OnInitialized: PASS");

		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
	}


	public void OnInitializeFailed(InitializationFailureReason error){
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}


	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args) {
		Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", 
			args.purchasedProduct.definition.id));

		RewardUser ();
		
		return PurchaseProcessingResult.Complete;
	}


	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", 
			product.definition.storeSpecificId, failureReason));
		
		PurchaseFailed ();
	}

	void RewardUser (){
		SessionManager.Instance.playerData.Revenue += 10;
		SessionManager.Instance.Save ();
	}

	void PurchaseFailed (){
		PopupModalManager.Instance.Show ("Purchase failed.", PopupCallback, "okay");
	}

	void PopupCallback (){
	}
}

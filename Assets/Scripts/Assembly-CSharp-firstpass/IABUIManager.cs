using Prime31;
using UnityEngine;

public class IABUIManager : MonoBehaviourGUI
{
	private void OnGUI()
	{
		beginColumn();
		if (GUILayout.Button("Initialize IAB"))
		{
			string text = "your public key from the Android developer portal here";
			text = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2fddncCpVwPU3m4ZzG8MfQTrxf3LBdjWwOV4LBRy2q4Kp/gPYi5QQaNJjsiQAbpIR51qSEJv9EomOi8+JZ4rWO52zOaLeumnKzpv++QVOllbGxaSwwSPDEZ0++eKmdsl5r+xzVvd20ey4n5tYotrRdYQfypZKYuHiMGvpsiIXf0rwv3yMNhVU7MDtbDgAs8zriBvPqCtkrRLnZdG+2dQEZ+hDPns0gO+N8y1V7odOHg4bDUceaK8al9DHcVKNItCMnOFyLHx++vKzHSLiXw2ojSUR1cfSbTkyyOTw9r9emHxxuGmc2/qWp7n/Qin1ksuAhyYFGOC9RClxxu1ygXKTQIDAQAB";
			GoogleIAB.init(text);
		}
		if (GUILayout.Button("Query Inventory"))
		{
			string[] skus = new string[4] { "com.prime31.testproduct", "android.test.purchased", "com.prime31.managedproduct", "com.prime31.testsubscription" };
			GoogleIAB.queryInventory(skus);
		}
		if (GUILayout.Button("Are subscriptions supported?"))
		{
			Debug.Log("subscriptions supported: " + GoogleIAB.areSubscriptionsSupported());
		}
		if (GUILayout.Button("Test Purchase with Test Product"))
		{
			GoogleIAB.purchaseProduct("android.test.purchased");
		}
		if (GUILayout.Button("Consume Test Purchase"))
		{
			GoogleIAB.consumeProduct("android.test.purchased");
		}
		if (GUILayout.Button("Test Unavailable Item"))
		{
			GoogleIAB.purchaseProduct("android.test.item_unavailable");
		}
		endColumn(true);
		if (GUILayout.Button("Purchase Real Product"))
		{
			GoogleIAB.purchaseProduct("com.prime31.testproduct", "payload that gets stored and returned");
		}
		if (GUILayout.Button("Purchase Real Subscription"))
		{
			GoogleIAB.purchaseProduct("com.prime31.testsubscription", "subscription payload");
		}
		if (GUILayout.Button("Consume Real Purchase"))
		{
			GoogleIAB.consumeProduct("com.prime31.testproduct");
		}
		if (GUILayout.Button("Purchase Real Subscription"))
		{
			GoogleIAB.purchaseProduct("com.prime31.testsubscription", "my subscription payload");
		}
		if (GUILayout.Button("Enable High Details Logs"))
		{
			GoogleIAB.enableLogging(true);
		}
		if (GUILayout.Button("Consume Multiple Purchases"))
		{
			string[] skus2 = new string[2] { "com.prime31.testproduct", "android.test.purchased" };
			GoogleIAB.consumeProducts(skus2);
		}
		endColumn();
	}
}

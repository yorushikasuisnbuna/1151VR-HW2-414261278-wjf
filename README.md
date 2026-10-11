


1151VR 作業二 Unity 2D 專案製作說明





一、場景建置
我先建立 2D 專案，在 SampleScene 中把人物圖片拖進場景，Unity 會自動產生名為 player_19_0 的 Sprite 物件。地面和平台則是用三個 Square 物件，分別上色成橘色、粉色與綠色，並且各自加上 Box Collider 2D，這樣人物才踩得到。三個 Square 的 Tag 都設為 Ground，後面的程式要靠這個 Tag 判斷人物是不是站在地上。
另外我在場景中放了兩個空物件，名稱是 checkpoint 1 和 Goal，它們沒有畫面，只用來標記位置。checkpoint 1 放在路線中途，Goal 放在綠色終點地面上。

<img width="955" height="435" alt="螢幕擷取畫面 2026-10-11 142820" src="https://github.com/user-attachments/assets/5b6d756e-2e1c-4d45-b553-c0402cfb3532" />

二、人物與相機的元件設定
人物身上除了 Sprite Renderer，還加了 Rigidbody 2D 與 Box Collider 2D。Rigidbody 2D 的 Body Type 是 Dynamic，Gravity Scale 為 1，所以人物會受重力掉到地面；我也勾選 Freeze Rotation Z，避免人物在平台邊緣翻倒。因為原本的圖片在畫面中太大，我把人物的 Scale 調成 0.3，並重新把位置拉到地面上。
相機使用 Orthographic 模式，Size 為 5。我另外寫了 CameraFollow 腳本掛在 Main Camera 上，讓相機跟著人物移動。



三、程式設計

<img width="959" height="430" alt="螢幕擷取畫面 2026-10-11 142741" src="https://github.com/user-attachments/assets/9ca5619c-34da-4487-93b3-161d17204a95" />


專案共有兩支腳本。
CameraFollow 有三個可調整的欄位：要跟隨的目標 Target、相機與人物之間的距離 Offset（我設為 0、2、-10，Z 軸一定要是負值，相機才看得到 2D 物件），以及跟隨的平滑程度 Smooth。程式寫在 LateUpdate 裡，每個畫面都用 Vector3.Lerp 把相機慢慢移到「人物位置加上 Offset」的地方。放在 LateUpdate 是為了等人物先移動完，相機再跟上，畫面才不會抖動。
PlayerController 是主要的控制腳本，分成幾個部分。
移動的部分，我用 Input.GetAxisRaw 讀取 A、D 或左右方向鍵的輸入，得到 -1、0 或 1，組成一個 Vector2 型別的 moveDir。接著用 transform.Translate 乘上 moveSpeed 與 Time.deltaTime，讓人物以固定速度行走。人物往左走時，圖片要跟著轉向，所以我把 localScale 的 x 取絕對值，再依方向乘上 1 或 -1，這樣就不會改變人物原本的大小。
跳躍的部分，按下空白鍵時，用 Rigidbody 2D 的 AddForce 往上施一個瞬間的力（ForceMode2D.Impulse），力道由 jumpForce 控制。為了避免人物在空中連續跳，我用布林變數 isGrounded 記錄人物有沒有站在地上：人物持續碰到 Tag 為 Ground 的物件時，OnCollisionStay2D 會把它設為 true，離開時 OnCollisionExit2D 會設回 false，只有 isGrounded 為 true 時才允許起跳。
檢查點的部分，我宣告了一個 Transform 陣列 checkpoints，這是作業要求的陣列。陣列依序放入 checkpoint 1 和 Goal，最後一個一定是終點。程式在每個畫面用 Vector2.Distance 計算人物與「下一個目標」的距離，如果小於 reachDistance，就算通過，並把 nextIndex 加一。通過 checkpoint 1 時在 Console 顯示訊息，通過最後一個Goal時，把 finished 設為 true、讓人物的速度歸零，並在 OnGUI 裡於畫面中央顯示「到達終點！」。finished 為 true 之後，Update 一開始就直接 return，所以人物不能再操作。

四、遇到的問題與解決方式
製作過程中遇到幾個問題，我整理如下。
第一，一開始人物完全無法移動。Unity 6 預設使用新版的輸入系統，而我的程式是用舊版的 Input 類別寫的，兩者不相容。我到 Project Settings 的 Player 頁面，把 Active Input Handling 改成 Both，並確認 Input System 套件已安裝，重新啟動 Unity 後才正常。
第二，跳躍沒有反應。我在按下空白鍵時加了一行 Debug.Log，印出 isGrounded 的值，才確認是落地判斷出了問題，後來把 OnCollisionEnter2D 改成 OnCollisionStay2D，並檢查每一塊地面的 Tag 都是 Ground，跳躍就正常了。
第三，走到終點沒有反應。我先確認 Inspector 裡 checkpoints 陣列的 Size 是 2，且依序放入 checkpoint 1 和 Goal，再從 Console 看不到「通過檢查點 1」，判斷是空物件的位置離人物行走的路線太遠，而人物必須先通過 checkpoint 1 才會判斷 Goal。最後我把 reachDistance 調大到 4，兩個檢查點都能正常觸發。

五、操作方式與成果
執行遊戲後，按 A、D 或左右方向鍵可以左右移動，人物會跟著轉向；站在地面上時按空白鍵可以跳躍；相機會隨著人物平滑地移動。經過 checkpoint 1 後繼續走到終點，人物會停下來，畫面顯示「到達終點！」


影片連結:　https://youtu.be/ShhOzIWQXis
Github連結: https://github.com/yorushikasuisnbuna/1151VR-HW2-414261278-wjf/tree/main








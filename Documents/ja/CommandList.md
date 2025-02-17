# コマンド一覧
|コマンド|機能の説明|変数の説明|
|---|---|---|
|`[actor name=A texture=B]`|立ち絵を表示する。|A: 立ち絵の名前<br>B: 画像のファイル名|
|`[actor name=A layout=center]`|立ち絵の位置を変える。|A: 立ち絵の名前<br>B: left center right|
|`[actor name=A x=123 y=123]`|立ち絵の位置を変える。|A: 立ち絵の名前<br>数値はピクセル数|
|`[actor name=A xoffset=123 yoffset=123]`|立ち絵の位置をずらす。|A: 立ち絵の名前<br>数値はピクセル数|
|`[actor name=A clear]`|立ち絵を非表示にする。|A: 立ち絵の名前|
|`[background texture=A]`|背景を表示する。|A: 画像のファイル名|
|`[choice text="A" label=B]`|分岐の選択肢を表示する。|A: ボタンの文章<br>B: 移動先のラベル|
|`[choice text="A" file=B label=C]`|分岐の選択肢を表示する。|A: ボタンの文章<br>B: 移動先のファイル名<br>C: 移動先のラベル|
|`[choice texture=A label=B]`|分岐の選択肢を表示する。<br>(画像バージョン)|A: 画像のファイル名<br>B: 移動先のラベル|
|`[clear]`|立ち絵と背景を全てを非表示にする。<br>章の始まりに使用する。||
|`[jump label=A]`|別の章に移動する。|A: 移動先のラベル|
|`[jump file=A label=B]`|別の章に移動する。|A: 移動先のファイル名<br>B: 移動先のラベル|
|`[r]`|改行する。|
|`[p]`|クリックを待つ。クリックするとテキストを削除する。|
|`[talker name=A]`|話者名を表示する。|A: 名前|
|`[talker clear]`|話者名を非表示にする。||
|`[wait time=1]`|待機する。|数値は秒数|

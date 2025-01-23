# コマンドの書き方
`[コマンド名 変数=値]` という形で書きます。
```
[actor name=A texture=A]
```

コマンド名が同じ場合は変数を連結して書くことができます。
```
[actor name=A texture=A]
[actor name=A layout=center]
  ↓
[actor name=A texture=A layout=center]
```

# コマンド一覧
|コマンド|説明|
|---|---|
|actor name=A texture=A|AというキャラにAという画像を設定する。|
|actor name=A layout=center|Aというキャラを真ん中に配置する。|
|background texture=A|"A"という画像を表示する。|

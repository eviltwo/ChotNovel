# コマンドの書き方
`[コマンド名 変数=値]` という形で書きます。
```
[actor name=A texture=A]
```

コマンドが同じなら変数を連結できます。
```
[actor name=A texture=A]
[actor name=A layout=center]
  ↓
[actor name=A texture=A layout=center]
```

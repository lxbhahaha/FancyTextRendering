<voffset=0.7em><u><b><size=2em>FancyTextRendering</size></b></u></voffset>
It's some code to render markdown & clickable links in Unity. How fun!

<voffset=0.3em><u><b><size=1.7em>We got multiple levels of headers</size></b></u></voffset>
Isn't that neat.

Anywho, we've got all your standard Markdown features. You can make stuff <i>italic</i> or <b>bold</b>, or even <i><b>both simultaneously</b></i>. You can cross out stuff with <s>strikethrough</s> or make it codey with <space=0.3em><font="Noto/Noto Mono/NotoMono-Regular"><mark=#0000003C padding="25,25,0,0">monospace</font></mark><space=0.3em>.

<b><size=1.5em>Lists!</size></b>
Boy oh boy do we have lists!
<line-height=0.76em></line-height>
<line-height=0%><width=100><align=right>•</width></align>
</line-height><indent=120>Unordered lists that use bullet points!</indent>
<line-height=0%><width=100><align=right>•</width></align>
</line-height><indent=120>See, it's another item in the list!</indent>
<line-height=0%><width=100><align=right>•</width></align>
</line-height><indent=120>Wowee, another one!!!</indent>
<line-height=0.76em></line-height>
Ordered lists are also supported :D
<line-height=0.76em></line-height>
<line-height=0%><width=100><align=right>1.</width></align>
</line-height><indent=120>If you use "1" for every number, the list is auto-numbered, in keeping with standard markdown rules.</indent>
<line-height=0%><width=100><align=right>2.</width></align>
</line-height><indent=120>This makes it easy to insert an item in the list without having to update the number for every single item in the list.</indent>
<line-height=0%><width=100><align=right>3.</width></align>
</line-height><indent=120>But if you want to control the numbers manually, that is also supported.</indent>
<line-height=0.76em></line-height>
See, like in this list:
<line-height=0.76em></line-height>
<line-height=0%><width=100><align=right>69.</width></align>
</line-height><indent=120>This is an intentional difference from the markdown spec.</indent>
<line-height=0%><width=100><align=right>420.</width></align>
</line-height><indent=120>I often see folks get confused by Markdown's list auto-numbering, and this change should ensure that the feature only applies to folks who know what they're doing.</indent>
<line-height=0.76em></line-height>
<b><size=1.5em>Links!</size></b>
HTTP and HTTPS links are automatically detected and added, like so: <color=#1D7CEAFF><link="http://isitwednesday.info/.">http://isitwednesday.info/.</link></color> We also have inline links, <color=#1D7CEAFF><link="https://www.youtube.com/watch?v=dQw4w9WgXcQ">like this</link></color>.

<b><size=1.5em>Superscript!</size></b>
We got super<sup>script!</sup> We got super<sup>script but it's long!</sup>

<b><size=1.5em>Escapes!</size></b>
You can *escape* control characters with a backslash. Regular^script!
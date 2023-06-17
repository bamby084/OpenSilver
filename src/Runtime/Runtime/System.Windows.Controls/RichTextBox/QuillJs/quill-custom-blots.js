const Inline = Quill.import('blots/inline');

class LinkBlot extends Inline {
	static create(options) {
		const node = super.create(options);
		node.innerHTML = options.text;
		node.setAttribute('href', '#');

		if (options.href) {
			node.setAttribute('data-href', options.href);
			node.addEventListener("click", function (e) {
				e.preventDefault();
				const editor = this.closest(".ql-editor");
				if (editor.getAttribute("contenteditable") == "true")
					return;

				if (options.href.startsWith("javascript:")) {
					eval(options.href);
				} else {
					window.open(options.href, '_blank').focus();
				}
			});
		}
		
		return node;
	}

	static formats(node) {
		return node.getAttribute('data-href');
	}
}

LinkBlot.blotName = 'hyperlink';
LinkBlot.tagName = 'a';
Quill.register(LinkBlot);
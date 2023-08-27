import {Component, Input} from '@angular/core';

@Component({
  selector: 'app-config-xml-summery',
  templateUrl: './config-xml-summery.component.html',
  styleUrls: ['./config-xml-summery.component.css']
})
export class ConfigXmlSummeryComponent {

  /**
   * The .NET xml summery.
   */
  private _xml: string = '';
  @Input()
  public get xml(): string {
    return this._xml;
  }
  public set xml(xml: string) {
    this._xml = xml;
    this.generateHtmlFromXml();
  }

  /**
   * The generated html.
   */
  private _html: string = '';
  public get html(): string {
    return this._html;
  }

  /**
   * Converts the .NET xml summery to html.
   */
  private generateHtmlFromXml() {

    const parser = new DOMParser();
    const document = parser.parseFromString(this._xml,"text/xml");

    this._html = this.generateHtmlFromNode(document);
  }

  private generateHtmlFromNode(node: Node): string {
    const element = node as Element;

    let html = '';
    let htmlTag = '';
    let htmlContent = '';

    switch (node.nodeName) {
      case 'para':
        htmlTag = 'p';
        break;
      case 'c':
        htmlTag = 'code';
        break;
      case 'see':
        const cref = element.getAttribute('cref') ?? '';

        // Shortens the name...
        let name = cref;
        const index = cref.lastIndexOf('.');
        if (index > 0)
          name = cref.substring(index + 1);

        htmlContent = name;
        htmlTag = 'code';
        break;
      case '#text':
        // We should escape html here...
        htmlContent = node.nodeValue ?? '';
        break;
    }

    if (htmlTag)
      html += '<' + htmlTag + '>';

    if (htmlContent)
      html += htmlContent;

    // Build all child nodes
    for (let i = 0; i < node.childNodes.length; i++) {
      const child = node.childNodes[i];
      html += this.generateHtmlFromNode(child);
    }

    if (htmlTag)
      html += '</' + htmlTag + '>';

    return html;
  }
}

import xml.etree.ElementTree as etree 

def GetAttributeText(node, attributeName):
    try:
        return node.attrib[attributeName]
    except:
        print "Exception occurred when attempting to access the \"" + attributeName + "\" attribute"
        return None

def GetNodeText(node):
    try:
        return node.text
    except:
        print "Exception occurred when attempting to access the node's text"
        return None

def ExtractChangelist(node, outputFile):   
    outputFile.write("  <tr class=\"pane\">\n")
    outputFile.write("    <td colspan=\"2\" class=\"changeset\">\n")
    outputFile.write("      <div class=\"changeset-message\">\n")
    outputFile.write("        <b>${%Version} " + GetAttributeText(node, 'version') + " by <a href=\"${rootURL}/${cs.author.url}/\">" + GetNodeText(node.find('user')) + "</a>:</b><br/>\n")
    outputFile.write("        " + GetNodeText(node.find('comment')) + "\n")
    outputFile.write("      </div>\n")
    outputFile.write("    </td>\n")
    outputFile.write("  </tr>\n")
    
    
    # foreach item in the cl
    itemsNode = node.find('items')
    if itemsNode is not None:
        for item in node.find("items"):
            outputFile.write("  <tr>\n")
            outputFile.write("    <td>" + GetAttributeText(item, 'action') + "</td>\n")
            outputFile.write("    <td>" + GetNodeText(item) + "</td>\n")
            outputFile.write("  </tr>\n")

def WriteHeaderHtml(outputFile):
    outputFile.write("<table class=\"pane\" style=\"border:none\">")

def WriteFooterHtml(outputFile):
    outputFile.write("</table>")

if __name__ == "__main__":
    src = "XmlSourceFile.xml"
    output = "Output.html"

    inputTree = etree.parse(src)   
    outputFile = open('Output.html', 'w')

    WriteHeaderHtml(outputFile)

    for node in inputTree.getroot():
        ExtractChangelist(node, outputFile)

    WriteFooterHtml(outputFile)
        
#<j:jelly xmlns:j="jelly:core" xmlns:st="jelly:stapler" xmlns:d="jelly:define" xmlns:l="/lib/layout" xmlns:t="/lib/hudson" xmlns:f="/lib/form">
#  <h2>${%Summary}</h2>
#  <ol>
#    <j:forEach var="cs" items="${it.logs}">
#      <li><st:out value="${cs.msg}"/></li>
#    </j:forEach>
#  </ol>
#  <table class="pane" style="border:none">
#    <j:forEach var="cs" items="${it.items}" varStatus="loop">
#      <tr class="pane">
#        <td colspan="2" class="changeset">
#          <a name="detail${loop.index}"></a>
#          <div class="changeset-message">
#            <b>
#              ${%Version} ${cs.version} by <a href="${rootURL}/${cs.author.url}/">${cs.author}</a>:
#            </b><br/>
#            ${cs.msgAnnotated}
#          </div>
#        </td>
#      </tr>
#      <j:forEach var="item" items="${cs.items}">
#        <tr>
#          <td><t:editTypeIcon type="${item.editType}" /></td>
#          <td>${item.path}</td>
#        </tr>
#      </j:forEach>
#    </j:forEach>
#  </table>
#</j:jelly>
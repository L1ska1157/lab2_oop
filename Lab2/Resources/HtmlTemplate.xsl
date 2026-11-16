<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

    <xsl:output method="html" encoding="utf-8" indent="yes"/>

    <xsl:decimal-format name="uk" decimal-separator="," grouping-separator="&#160;"/>

    <xsl:template match="/scientists">
        <html>
            <head>
                <meta charset="utf-8"/>
                <title>Кадри науковців</title>
                <style>
                    body { font-family: sans-serif; }
                    table { border-collapse: collapse; }
                    th, td { border: 1px solid #888; padding: 4px 8px; }
                    th { background: #eee; }
                    td.num { text-align: right; }
                </style>
            </head>
            <body>
                <h1>Кадри науковців</h1>
                <table>
                    <tr>
                        <th>ПІБ</th>
                        <th>Факультет</th>
                        <th>Кафедра</th>
                        <th>Посада</th>
                        <th>Оклад, грн</th>
                        <th>Прийнятий</th>
                        <th>Звільнений</th>
                    </tr>
                    <xsl:apply-templates select="scientist"/>
                </table>
            </body>
        </html>
    </xsl:template>

    <xsl:template match="scientist">
        <tr>
            <td><xsl:value-of select="fullName"/></td>
            <td><xsl:value-of select="@faculty"/></td>
            <td><xsl:value-of select="@department"/></td>
            <td><xsl:value-of select="@position"/></td>
            <td class="num"><xsl:value-of select="format-number(@salary, '# ##0,00', 'uk')"/></td>
            <td>
                <xsl:call-template name="date">
                    <xsl:with-param name="iso" select="@hireDate"/>
                </xsl:call-template>
            </td>
            <td>
                <xsl:choose>
                    <xsl:when test="@fireDate">
                        <xsl:call-template name="date">
                            <xsl:with-param name="iso" select="@fireDate"/>
                        </xsl:call-template>
                    </xsl:when>
                    <xsl:otherwise>Працює</xsl:otherwise>
                </xsl:choose>
            </td>
        </tr>
    </xsl:template>

    <!-- yyyy-MM-dd -> dd.MM.yyyy -->
    <xsl:template name="date">
        <xsl:param name="iso"/>
        <xsl:value-of select="concat(substring($iso, 9, 2), '.', substring($iso, 6, 2), '.', substring($iso, 1, 4))"/>
    </xsl:template>

</xsl:stylesheet>
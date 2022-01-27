# REST API Security Headers

taken from [OWASP REST Security Cheat sheat](https://cheatsheetseries.owasp.org/cheatsheets/REST_Security_Cheat_Sheet.html)

| Header                                            | Rationale                                                                                             |
| ---                                               | ---                                                                                                   |
| Cache-Control: no-store	                        | Prevent sensitive information from being cached.                                                      |
| Content-Security-Policy: frame-ancestors 'none'	| To protect against drag-and-drop style clickjacking attacks.                                          |
| Content-Type	                                    | To specify the content type of the response. This should be application/json for JSON responses.      |
| Strict-Transport-Security	                        | To require connections over HTTPS and to protect against spoofed certificates.                        |
| X-Content-Type-Options: nosniff	                | To prevent browsers from performing MIME sniffing, and inappropriately interpreting responses as HTML.|
| X-Frame-Options: DENY	                            | To protect against drag-and-drop style clickjacking attacks.                                          |
| Content-Security-Policy: default-src 'none'	    | The majority of CSP functionality only affects pages rendered as HTML.                                |
| Feature-Policy: 'none'	                        | Feature policies only affect pages rendered as HTML.                                                  |
| Referrer-Policy: no-referrer	                    | Non-HTML responses should not trigger additional requests.                                            |

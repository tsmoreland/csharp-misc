package tsmoreland.authsample.consumer;

import org.apache.http.auth.AuthSchemeProvider;
import org.apache.http.auth.AuthScope;
import org.apache.http.auth.Credentials;
import org.apache.http.auth.NTCredentials;
import org.apache.http.client.CredentialsProvider;
import org.apache.http.client.config.AuthSchemes;
import org.apache.http.client.config.RequestConfig;
import org.apache.http.client.methods.CloseableHttpResponse;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.config.Registry;
import org.apache.http.config.RegistryBuilder;
import org.apache.http.impl.auth.SPNegoSchemeFactory;
import org.apache.http.impl.client.BasicCredentialsProvider;
import org.apache.http.impl.client.CloseableHttpClient;
import org.apache.http.impl.client.HttpClientBuilder;
import org.apache.http.impl.client.HttpClients;
import org.springframework.boot.CommandLineRunner;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.context.ConfigurableApplicationContext;

import java.security.Security;
import java.util.List;
import java.util.Scanner;

@SpringBootApplication
public class Application implements CommandLineRunner {

    public static void main(String[] args) {
        System.setProperty("java.security.krb5.conf", "/etc/krb5.conf");
        System.setProperty("java.security.auth.login.config", "login.conf");
        System.setProperty("javax.security.auth.useSubjectCredsOnly", "false");
        System.setProperty("sun.security.krb5.debug", "true");
        System.setProperty("sun.security.jgss.debug", "true");
        Security.setProperty("auth.login.defaultCallbackHandler", "net.curiousprogrammer.auth.kerberos.example.KerberosCallBackHandler");

        try (ConfigurableApplicationContext context = SpringApplication.run(Application.class, args)) {

            if (context.isActive()) {
                System.out.println("Context is active");
            }
        }
    }

    @Override
    public void run(String... args) throws Exception {

        //hello();
        helloUsingJaas();
    }

    private void printResponse(CloseableHttpResponse response) throws Exception {

        Scanner sc = new Scanner(response.getEntity().getContent());

        //Printing the status line
        System.out.println(response.getStatusLine());
        while(sc.hasNext()) {
            System.out.println(sc.nextLine());
        }
    }

    private void hello() {

        NTCredentials credentials = new NTCredentials("", "", "", "");
        // attempt to match default credentials
        CredentialsProvider credsProvider = new BasicCredentialsProvider();
        credsProvider.setCredentials(AuthScope.ANY, credentials);

        RequestConfig requestConfig = RequestConfig.custom()
            .setSocketTimeout(30000)
            .setConnectTimeout(30000)
            .setTargetPreferredAuthSchemes(List.of(AuthSchemes.KERBEROS))
            .setProxyPreferredAuthSchemes(List.of(AuthSchemes.BASIC))
            .build();

        try (CloseableHttpClient client = HttpClientBuilder.
                create().
                setDefaultCredentialsProvider(credsProvider).
                setDefaultRequestConfig(requestConfig).
                build()) {

            var request = new HttpGet("http://localhost:5071/api/hello");
            try (var response = client.execute(request)) {
                printResponse(response);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    private void helloUsingJaas() {
        var use_jaas_creds = new Credentials() {
            public String getPassword() {
                return null;
            }

            public java.security.Principal getUserPrincipal() {
                return null;
            }
        };

        CredentialsProvider credsProvider = new BasicCredentialsProvider();
        credsProvider.setCredentials(new AuthScope(null, -1, null), use_jaas_creds);
        Registry<AuthSchemeProvider> authSchemeRegistry = RegistryBuilder.<AuthSchemeProvider>create().register(AuthSchemes.SPNEGO, new SPNegoSchemeFactory(true)).build();
        try (var client = HttpClients.custom()
            .setDefaultAuthSchemeRegistry(authSchemeRegistry)
            .setDefaultCredentialsProvider(credsProvider).build()) {

            var request = new HttpGet("http://localhost:5071/api/hello");
            try (CloseableHttpResponse response = client.execute(request)) {
                printResponse(response);
            }

        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}

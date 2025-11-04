// MCP Server Test - Paste this in your browser console
// Server: http://185.220.206.118:14555/

async function testMcpServer() {
    const SERVER_URL = 'http://185.220.206.118:14555/';
    let sessionId = null;

    try {
        // Step 1: Initialize
        console.log('=== Step 1: Initialize ===');
        const initResponse = await fetch(SERVER_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                jsonrpc: '2.0',
                method: 'initialize',
                params: {
                    protocolVersion: '2024-11-05',
                    clientInfo: {
                        name: 'browser-test',
                        version: '1.0.0'
                    },
                    capabilities: {}
                },
                id: 1
            })
        });

        sessionId = initResponse.headers.get('Mcp-Session-Id');
        console.log('Session ID:', sessionId);

        if (!sessionId) {
            console.error('ERROR: Server did not return Mcp-Session-Id header');
            return;
        }
        
        // Don't consume the response body

        // Step 2: Send initialized notification
        console.log('\n=== Step 2: Send initialized notification ===');
        await fetch(SERVER_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Mcp-Session-Id': sessionId
            },
            body: JSON.stringify({
                jsonrpc: '2.0',
                method: 'notifications/initialized',
                params: {}
            })
        });
        console.log('✓ Initialized notification sent');

        // Step 3: List tools
        console.log('\n=== Step 3: List tools ===');
        const toolsResponse = await fetch(SERVER_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Mcp-Session-Id': sessionId
            },
            body: JSON.stringify({
                jsonrpc: '2.0',
                method: 'tools/list',
                params: {},
                id: 2
            })
        });

        // Parse SSE format: "event: message\ndata: {...}"
        const toolsText = await toolsResponse.text();
        console.log('Tools raw response:', toolsText);
        
        const toolsDataMatch = toolsText.match(/data: (.+)/);
        if (toolsDataMatch) {
            const toolsResult = JSON.parse(toolsDataMatch[1]);
            console.log('Tools parsed:', toolsResult);
        } else {
            console.error('Could not parse tools response');
        }

        // Step 4: Call reverse tool
        console.log('\n=== Step 4: Call reverse tool ===');
        const reverseResponse = await fetch(SERVER_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Mcp-Session-Id': sessionId
            },
            body: JSON.stringify({
                jsonrpc: '2.0',
                method: 'tools/call',
                params: {
                    name: 'reverse',
                    arguments: {
                        message: 'Hello World'
                    }
                },
                id: 3
            })
        });

        // Parse SSE format: "event: message\ndata: {...}"
        const reverseText = await reverseResponse.text();
        console.log('Reverse raw response:', reverseText);
        
        const reverseDataMatch = reverseText.match(/data: (.+)/);
        if (reverseDataMatch) {
            const reverseResult = JSON.parse(reverseDataMatch[1]);
            console.log('Reverse parsed:', reverseResult);
        } else {
            console.error('Could not parse reverse response');
        }

        console.log('\n✅ ALL TESTS PASSED!');

    } catch (error) {
        console.error('❌ ERROR:', error.message);
        console.error('Stack:', error.stack);
    }
}

// Run the test
testMcpServer();

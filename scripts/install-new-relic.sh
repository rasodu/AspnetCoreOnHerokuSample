#!/bin/sh
#Line ending in this fie must be unix line ending. 'vi <file>' => :set fileformat=unix => wq
set -e
set -o verbose
echo "This command will be executed during building Web image. It will install New Relic. Define 'NEW_RELIC_LICENSE_KEY' and 'NEW_RELIC_APP_NAME' for your applicaiton."
apt-get update && apt-get install wget
wget -O newrelic-netcore20-agent.deb 'https://download.newrelic.com/dot_net_agent/latest_release/newrelic-netcore20-agent_8.12.216.0_amd64.deb'
dpkg -i newrelic-netcore20-agent.deb
rm newrelic-netcore20-agent.deb

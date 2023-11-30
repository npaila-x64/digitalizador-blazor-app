# Don't change
SRC_DIR := src

.PHONY: start
start:
	node ./server.js

# Build all services
.PHONY: build
build:
	@docker-compose build

# Start up all services
.PHONY: up
up:
	@docker-compose up -d

# Tear down all services
.PHONY: down
down:
	@docker-compose down

# Remove all db data
.PHONY: remove-data
remove-data:
	@if [ $$(docker volume ls -q -f name=solicitudes-service_solicitudes-mongodata) ]; then \
		docker volume rm solicitudes-service_solicitudes-mongodata; \
	else \
		echo "Volume solicitudes-service_solicitudes-mongodata does not exist, skipping removal"; \
	fi

# Remove all older setup and start fresh services
.PHONY: recreate
recreate: down remove-data build up

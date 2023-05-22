#!/bin/bash
docker-compose pull
docker-compose -f docker-compose.yml up -d --force-recreate --remove-orphans

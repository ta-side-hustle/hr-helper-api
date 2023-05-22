#!/bin/bash
git pull
./update-image.sh
./apply-migrations.sh

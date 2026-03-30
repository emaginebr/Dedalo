-- =============================================
-- Dedalo CMS - Database Schema
-- PostgreSQL 17
-- =============================================

-- dedalo_websites
CREATE TABLE dedalo_websites (
    website_id BIGSERIAL NOT NULL,
    user_id BIGINT NOT NULL,
    website_slug VARCHAR(240) NOT NULL,
    template_slug VARCHAR(240),
    name VARCHAR(240) NOT NULL,
    domain_type INTEGER NOT NULL DEFAULT 1,
    custom_domain VARCHAR(240),
    logo_url VARCHAR(500),
    css TEXT,
    status INTEGER NOT NULL DEFAULT 1,
    created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    updated_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    CONSTRAINT dedalo_websites_pkey PRIMARY KEY (website_id)
);

CREATE UNIQUE INDEX ix_dedalo_websites_slug ON dedalo_websites (website_slug);
CREATE UNIQUE INDEX ix_dedalo_websites_custom_domain ON dedalo_websites (custom_domain) WHERE custom_domain IS NOT NULL AND custom_domain <> '';
CREATE INDEX ix_dedalo_websites_user_id ON dedalo_websites (user_id);

-- dedalo_pages
CREATE TABLE dedalo_pages (
    page_id BIGSERIAL NOT NULL,
    website_id BIGINT NOT NULL,
    page_slug VARCHAR(240) NOT NULL,
    template_page_slug VARCHAR(240),
    name VARCHAR(240) NOT NULL,
    created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    updated_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    CONSTRAINT dedalo_pages_pkey PRIMARY KEY (page_id),
    CONSTRAINT fk_dedalo_page_website FOREIGN KEY (website_id)
        REFERENCES dedalo_websites (website_id) ON DELETE CASCADE
);

CREATE INDEX ix_dedalo_pages_website_id ON dedalo_pages (website_id);

-- dedalo_menus
CREATE TABLE dedalo_menus (
    menu_id BIGSERIAL NOT NULL,
    website_id BIGINT NOT NULL,
    parent_id BIGINT,
    name VARCHAR(240) NOT NULL,
    link_type INTEGER NOT NULL DEFAULT 1,
    external_link VARCHAR(500),
    page_id BIGINT,
    created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    updated_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    CONSTRAINT dedalo_menus_pkey PRIMARY KEY (menu_id),
    CONSTRAINT fk_dedalo_menu_website FOREIGN KEY (website_id)
        REFERENCES dedalo_websites (website_id) ON DELETE CASCADE,
    CONSTRAINT fk_dedalo_menu_page FOREIGN KEY (page_id)
        REFERENCES dedalo_pages (page_id) ON DELETE SET NULL,
    CONSTRAINT fk_dedalo_menu_parent FOREIGN KEY (parent_id)
        REFERENCES dedalo_menus (menu_id) ON DELETE SET NULL
);

CREATE INDEX ix_dedalo_menus_website_id ON dedalo_menus (website_id);
CREATE INDEX ix_dedalo_menus_page_id ON dedalo_menus (page_id);
CREATE INDEX ix_dedalo_menus_parent_id ON dedalo_menus (parent_id);

-- dedalo_contents
CREATE TABLE dedalo_contents (
    content_id BIGSERIAL NOT NULL,
    website_id BIGINT NOT NULL,
    page_id BIGINT NOT NULL,
    content_type VARCHAR(100) NOT NULL,
    index INTEGER NOT NULL DEFAULT 0,
    content_slug VARCHAR(240),
    content_value TEXT,
    created_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    updated_at TIMESTAMP WITHOUT TIME ZONE NOT NULL,
    CONSTRAINT dedalo_contents_pkey PRIMARY KEY (content_id),
    CONSTRAINT fk_dedalo_content_website FOREIGN KEY (website_id)
        REFERENCES dedalo_websites (website_id) ON DELETE CASCADE,
    CONSTRAINT fk_dedalo_content_page FOREIGN KEY (page_id)
        REFERENCES dedalo_pages (page_id) ON DELETE CASCADE
);

CREATE INDEX ix_dedalo_contents_website_id ON dedalo_contents (website_id);
CREATE INDEX ix_dedalo_contents_page_id ON dedalo_contents (page_id);

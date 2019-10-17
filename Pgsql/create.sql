-- Database generated with pgModeler (PostgreSQL Database Modeler).
-- pgModeler  version: 0.9.2-beta
-- PostgreSQL version: 11.0
-- Project Site: pgmodeler.io
-- Model Author: ---

SET check_function_bodies = false;
-- ddl-end --


-- Database creation must be done outside a multicommand file.
-- These commands were put in this file only as a convenience.
-- -- object: station | type: DATABASE --
-- -- DROP DATABASE IF EXISTS station;
-- CREATE DATABASE station;
-- -- ddl-end --
-- 

-- object: public.users | type: TABLE --
-- DROP TABLE IF EXISTS public.users CASCADE;
CREATE TABLE public.users (
	id serial NOT NULL,
	email varchar(32) NOT NULL,
	password varchar(32) NOT NULL,
	display_name varchar(16),
	CONSTRAINT "UQ_users" UNIQUE (email,display_name),
	CONSTRAINT users_pk PRIMARY KEY (id)

);
-- ddl-end --
ALTER TABLE public.users OWNER TO postgres;
-- ddl-end --

-- object: public.songs | type: TABLE --
-- DROP TABLE IF EXISTS public.songs CASCADE;
CREATE TABLE public.songs (
	id serial NOT NULL,
	title varchar NOT NULL,
	duration integer NOT NULL,
	genres varchar[],
	mbid char(36),
	id_albums integer NOT NULL,
	id_libraries integer,
	CONSTRAINT songs_pk PRIMARY KEY (id)

);
-- ddl-end --
ALTER TABLE public.songs OWNER TO postgres;
-- ddl-end --

-- object: public.album_types | type: TYPE --
-- DROP TYPE IF EXISTS public.album_types CASCADE;
CREATE TYPE public.album_types AS
 ENUM ('album','single','mixtape','unreleased','bootleg');
-- ddl-end --
ALTER TYPE public.album_types OWNER TO postgres;
-- ddl-end --

-- object: public.albums | type: TABLE --
-- DROP TABLE IF EXISTS public.albums CASCADE;
CREATE TABLE public.albums (
	id serial NOT NULL,
	title varchar NOT NULL,
	cover_art varchar,
	type public.album_types NOT NULL DEFAULT 'album',
	tags varchar[],
	mbid char(36),
	id_artists integer,
	CONSTRAINT albums_pk PRIMARY KEY (id)

);
-- ddl-end --
ALTER TABLE public.albums OWNER TO postgres;
-- ddl-end --

-- object: public.artists | type: TABLE --
-- DROP TABLE IF EXISTS public.artists CASCADE;
CREATE TABLE public.artists (
	id serial NOT NULL,
	title varchar NOT NULL,
	tags varchar[],
	mbid char(36),
	cover_art varchar,
	CONSTRAINT artists_pk PRIMARY KEY (id)

);
-- ddl-end --
ALTER TABLE public.artists OWNER TO postgres;
-- ddl-end --

-- object: public.many_artists_has_many_songs | type: TABLE --
-- DROP TABLE IF EXISTS public.many_artists_has_many_songs CASCADE;
CREATE TABLE public.many_artists_has_many_songs (
	id_artists integer NOT NULL,
	id_songs integer NOT NULL,
	CONSTRAINT many_artists_has_many_songs_pk PRIMARY KEY (id_artists,id_songs)

);
-- ddl-end --

-- object: artists_fk | type: CONSTRAINT --
-- ALTER TABLE public.many_artists_has_many_songs DROP CONSTRAINT IF EXISTS artists_fk CASCADE;
ALTER TABLE public.many_artists_has_many_songs ADD CONSTRAINT artists_fk FOREIGN KEY (id_artists)
REFERENCES public.artists (id) MATCH FULL
ON DELETE RESTRICT ON UPDATE CASCADE;
-- ddl-end --

-- object: songs_fk | type: CONSTRAINT --
-- ALTER TABLE public.many_artists_has_many_songs DROP CONSTRAINT IF EXISTS songs_fk CASCADE;
ALTER TABLE public.many_artists_has_many_songs ADD CONSTRAINT songs_fk FOREIGN KEY (id_songs)
REFERENCES public.songs (id) MATCH FULL
ON DELETE RESTRICT ON UPDATE CASCADE;
-- ddl-end --

-- object: albums_fk | type: CONSTRAINT --
-- ALTER TABLE public.songs DROP CONSTRAINT IF EXISTS albums_fk CASCADE;
ALTER TABLE public.songs ADD CONSTRAINT albums_fk FOREIGN KEY (id_albums)
REFERENCES public.albums (id) MATCH FULL
ON DELETE RESTRICT ON UPDATE CASCADE;
-- ddl-end --

-- object: public.libraries | type: TABLE --
-- DROP TABLE IF EXISTS public.libraries CASCADE;
CREATE TABLE public.libraries (
	id serial NOT NULL,
	name varchar NOT NULL,
	id_users integer,
	CONSTRAINT libraries_pk PRIMARY KEY (id)

);
-- ddl-end --
ALTER TABLE public.libraries OWNER TO postgres;
-- ddl-end --

-- object: users_fk | type: CONSTRAINT --
-- ALTER TABLE public.libraries DROP CONSTRAINT IF EXISTS users_fk CASCADE;
ALTER TABLE public.libraries ADD CONSTRAINT users_fk FOREIGN KEY (id_users)
REFERENCES public.users (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: libraries_fk | type: CONSTRAINT --
-- ALTER TABLE public.songs DROP CONSTRAINT IF EXISTS libraries_fk CASCADE;
ALTER TABLE public.songs ADD CONSTRAINT libraries_fk FOREIGN KEY (id_libraries)
REFERENCES public.libraries (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: public.user_library_follows | type: TABLE --
-- DROP TABLE IF EXISTS public.user_library_follows CASCADE;
CREATE TABLE public.user_library_follows (
	id_users integer NOT NULL,
	id_libraries integer NOT NULL,
	CONSTRAINT user_library_follows_pk PRIMARY KEY (id_users,id_libraries)

);
-- ddl-end --

-- object: users_fk | type: CONSTRAINT --
-- ALTER TABLE public.user_library_follows DROP CONSTRAINT IF EXISTS users_fk CASCADE;
ALTER TABLE public.user_library_follows ADD CONSTRAINT users_fk FOREIGN KEY (id_users)
REFERENCES public.users (id) MATCH FULL
ON DELETE CASCADE ON UPDATE CASCADE;
-- ddl-end --

-- object: libraries_fk | type: CONSTRAINT --
-- ALTER TABLE public.user_library_follows DROP CONSTRAINT IF EXISTS libraries_fk CASCADE;
ALTER TABLE public.user_library_follows ADD CONSTRAINT libraries_fk FOREIGN KEY (id_libraries)
REFERENCES public.libraries (id) MATCH FULL
ON DELETE CASCADE ON UPDATE CASCADE;
-- ddl-end --

-- object: artists_fk | type: CONSTRAINT --
-- ALTER TABLE public.albums DROP CONSTRAINT IF EXISTS artists_fk CASCADE;
ALTER TABLE public.albums ADD CONSTRAINT artists_fk FOREIGN KEY (id_artists)
REFERENCES public.artists (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: public.createuser | type: FUNCTION --
-- DROP FUNCTION IF EXISTS public.createuser(varchar,varchar,varchar) CASCADE;
CREATE FUNCTION public.createuser ( email varchar,  password varchar,  display_name varchar)
	RETURNS public.users
	LANGUAGE plpgsql
	VOLATILE 
	CALLED ON NULL INPUT
	SECURITY INVOKER
	COST 1
	AS $$
DECLARE
  _rec public.users;
BEGIN
  INSERT INTO users (email, password, display_name)
  VALUES (email, password, display_name)
  RETURNING users.id, users.email, users.password, users.display_name
  INTO _rec;
  RETURN _rec;
END;
$$;
-- ddl-end --
ALTER FUNCTION public.createuser(varchar,varchar,varchar) OWNER TO postgres;
-- ddl-end --

-- object: public.getuser | type: FUNCTION --
-- DROP FUNCTION IF EXISTS public.getuser(integer) CASCADE;
CREATE FUNCTION public.getuser ( user_id integer)
	RETURNS TABLE ( id integer,  display_name varchar)
	LANGUAGE plpgsql
	VOLATILE 
	CALLED ON NULL INPUT
	SECURITY INVOKER
	COST 1
	ROWS 1
	AS $$
BEGIN
  RETURN QUERY
  SELECT users.id, users.display_name
  FROM users
  WHERE users.id = user_id;
END;
$$;
-- ddl-end --
ALTER FUNCTION public.getuser(integer) OWNER TO postgres;
-- ddl-end --

-- object: public.getusers | type: FUNCTION --
-- DROP FUNCTION IF EXISTS public.getusers() CASCADE;
CREATE FUNCTION public.getusers ()
	RETURNS TABLE ( id integer,  display_name varchar)
	LANGUAGE plpgsql
	VOLATILE 
	CALLED ON NULL INPUT
	SECURITY INVOKER
	COST 1
	AS $$
BEGIN
  RETURN QUERY
  SELECT users.id, users.display_name
  FROM users
  LIMIT 20;
END;
$$;
-- ddl-end --
ALTER FUNCTION public.getusers() OWNER TO postgres;
-- ddl-end --

-- object: public.createlibrary | type: FUNCTION --
-- DROP FUNCTION IF EXISTS public.createlibrary(varchar,integer) CASCADE;
CREATE FUNCTION public.createlibrary ( name varchar,  owner_id integer)
	RETURNS public.libraries
	LANGUAGE plpgsql
	VOLATILE 
	CALLED ON NULL INPUT
	SECURITY INVOKER
	COST 1
	AS $$
DECLARE
  _rec record;
BEGIN  
  INSERT INTO libraries (name, owner_id)
  VALUES (name, owner_id)
  RETURNING libraries.id, libraries.name, libraries.owner_id
  INTO _rec;
  RETURN _rec;
END;
$$;
-- ddl-end --
ALTER FUNCTION public.createlibrary(varchar,integer) OWNER TO postgres;
-- ddl-end --

-- object: public.getlibrary | type: FUNCTION --
-- DROP FUNCTION IF EXISTS public.getlibrary(integer) CASCADE;
CREATE FUNCTION public.getlibrary ( id integer)
	RETURNS TABLE ( id integer,  name varchar,  owner_id integer,  owner_display_name varchar)
	LANGUAGE plpgsql
	VOLATILE 
	CALLED ON NULL INPUT
	SECURITY INVOKER
	COST 1
	AS $$
BEGIN
  RETURN QUERY
  SELECT 
    libraries.id,
    libraries.name,
    libraries.owner_id,
    users.display_name as owner_display_name
  FROM libraries
  LEFT JOIN users ON users.id = libraries.owner_id
  WHERE libraries.id = id;
END;
$$;
-- ddl-end --
ALTER FUNCTION public.getlibrary(integer) OWNER TO postgres;
-- ddl-end --

-- object: public.getlibraries | type: FUNCTION --
-- DROP FUNCTION IF EXISTS public.getlibraries() CASCADE;
CREATE FUNCTION public.getlibraries ()
	RETURNS TABLE ( id integer,  name varchar,  owner_id integer,  owner_display_name varchar)
	LANGUAGE plpgsql
	VOLATILE 
	CALLED ON NULL INPUT
	SECURITY INVOKER
	COST 1
	AS $$
BEGIN
  RETURN QUERY
  SELECT 
    libraries.id,
    libraries.name,
    libraries.owner_id,
    users.display_name as owner_display_name
  FROM libraries
  LEFT JOIN users ON users.id = libraries.owner_id
  LIMIT 20;
END;
$$;
-- ddl-end --
ALTER FUNCTION public.getlibraries() OWNER TO postgres;
-- ddl-end --

-- object: public.uploads | type: TABLE --
-- DROP TABLE IF EXISTS public.uploads CASCADE;
CREATE TABLE public.uploads (
	id serial NOT NULL,
	filename varchar,
	extension varchar,
	type smallint,
	size integer,
	id_songs integer,
	CONSTRAINT uploads_pk PRIMARY KEY (id)

);
-- ddl-end --
ALTER TABLE public.uploads OWNER TO postgres;
-- ddl-end --

-- object: songs_fk | type: CONSTRAINT --
-- ALTER TABLE public.uploads DROP CONSTRAINT IF EXISTS songs_fk CASCADE;
ALTER TABLE public.uploads ADD CONSTRAINT songs_fk FOREIGN KEY (id_songs)
REFERENCES public.songs (id) MATCH FULL
ON DELETE SET NULL ON UPDATE CASCADE;
-- ddl-end --

-- object: public.createupload | type: FUNCTION --
-- DROP FUNCTION IF EXISTS public.createupload(varchar,varchar,integer,integer) CASCADE;
CREATE FUNCTION public.createupload ( filename varchar,  extension varchar,  type integer,  size integer)
	RETURNS public.uploads
	LANGUAGE plpgsql
	VOLATILE 
	RETURNS NULL ON NULL INPUT
	SECURITY INVOKER
	COST 1
	AS $$
DECLARE
  _rec public.uploads;
BEGIN
  INSERT INTO uploads (filename, extension, type, size)
  VALUES (filename, extension, type, size)
  RETURNING public.uploads.*
  INTO _rec;
  RETURN _rec;
END;

$$;
-- ddl-end --
ALTER FUNCTION public.createupload(varchar,varchar,integer,integer) OWNER TO postgres;
-- ddl-end --

-- object: public.createsong | type: FUNCTION --
-- DROP FUNCTION IF EXISTS public.createsong(varchar,integer,varchar[],integer,integer[],char(36)) CASCADE;
CREATE FUNCTION public.createsong ( title varchar,  duration integer,  genres varchar[],  album_id integer,  artists integer[],  mbid char(36) DEFAULT NULL)
	RETURNS public.songs
	LANGUAGE plpgsql
	VOLATILE 
	CALLED ON NULL INPUT
	SECURITY INVOKER
	COST 1
	AS $$
DECLARE
  _rec public.songs;
BEGIN
  INSERT INTO songs (title, duration, genres, mbid, album_id)
  VALUES (title, duration, genres, mbid, album_id)
  RETURNING public.songs.*
  INTO _rec;

  FOREACH artist IN ARRAY artists
  LOOP
    INSERT INTO many_artists_has_many_songs (id_artists, id_songs)
    VALUES (artist, _rec.id);
  END LOOP;
  RETURN _rec;
END;

$$;
-- ddl-end --
ALTER FUNCTION public.createsong(varchar,integer,varchar[],integer,integer[],char(36)) OWNER TO postgres;
-- ddl-end --

-- object: public.createartist | type: FUNCTION --
-- DROP FUNCTION IF EXISTS public.createartist(varchar,varchar[],char(36),varchar) CASCADE;
CREATE FUNCTION public.createartist ( title varchar,  tags varchar[],  mbid char(36) DEFAULT NULL,  cover_art varchar DEFAULT NULL)
	RETURNS public.artists
	LANGUAGE plpgsql
	VOLATILE 
	CALLED ON NULL INPUT
	SECURITY INVOKER
	COST 1
	AS $$
DECLARE
  _rec public.artists;
BEGIN
  INSERT INTO artists (title, tags, mbid, cover_art)
  VALUES (title, tags, mbid, cover_art)
  RETURNING public.artists.*
  INTO _rec;
  RETURN _rec;
END;

$$;
-- ddl-end --
ALTER FUNCTION public.createartist(varchar,varchar[],char(36),varchar) OWNER TO postgres;
-- ddl-end --



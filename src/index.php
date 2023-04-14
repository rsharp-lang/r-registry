<?php 

define("pkg_repo", "/opt/upload/packages");

class App {

    /**
     * upload package to registry server
     * 
     * @uses api
    */
    public function upload() {
        $file = $_FILES["pkg"];
        $tmpzip = $file["tempfile"];
        $zip = new ZipArchive();
        $zip->open($tmpzip, ZipArchive::RDONLY);
        $content = $zip->getFromName("/index.json");
        $content = json_decode($content);
        $pkgName = $content["package"];
        $ver     = $content["version"];
        $pkgfile = pkg_repo . "/{$pkgName[0]}/{$pkgName}/{$ver}.zip";

        move_uploaded_file($tmpzip, $pkgfile);
        self::index_package($pkgfile);

        $zip->close();

        controller::success(1);
    }

    /**
     * write package file into registry
    */
    private function index_package($pkgfile) {
        # extract the html help document
        # write metadata to database
    }

    /**
     * download the package file
     * 
     * @uses file
    */
    public function download($pkg, $ver = "latest") {
        $pkgfile = NULL;
        $registry = new Table("registry");
        $query = [
            "package" => $pkg
        ];

        if ($ver == "latest") {
            $pkgfile = $registry->left_join("publish")->on([
                "registry" => "id",
                "publish" => "pkg"
            ])->where($query)->order_by("time", true)
            ->find();
        } else {
            $query["version"] = $ver;
            $pkgfile = $registry->left_join("publish")->on([
                "registry" => "id",
                "publish" => "pkg"
            ])->where($query)->find();
        }

        if (Utils::isDbNull($pkgfile)) {
            controller::error("package not found!", 404);
        } else {
            $path = $pkgfile["savefile"];
            Utils::PushDownload($path);
        }
    }
}